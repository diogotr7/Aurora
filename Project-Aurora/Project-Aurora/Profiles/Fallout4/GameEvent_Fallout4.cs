using Aurora.Utils;
using Aurora.Profiles.Fallout4.GSI;
using PipBoy;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Aurora.Profiles.Fallout4.GSI.Nodes;

namespace Aurora.Profiles.Fallout4
{
    public class GameEvent_Fallout4 : LightEvent
    {
        private GameStateReader reader;
        private PipBoyStreamProvider provider;

        #region Reflection Wizardry
        private readonly PropertyInfo[] nodes;
        private readonly Dictionary<PropertyInfo, FieldInfo[]> fields;
        private readonly Dictionary<string, PropertyAccessor> nodeAccessors;
        private readonly Dictionary<string, FieldAccessor> fieldAccessors;
        #endregion

        public GameEvent_Fallout4()
        {
            #region Reflection Wizardry 2: Electric Boogaloo
            /*
             * The following is done to limit the hardcoding done in the update tick.
             * It takes the names of the fields  & props from all the GSI Nodes
             * and creates accessors for them. This allows us to loop through all the properties,
             * accessing the respective getters and setters by name while being faster than using reflection.
             */
            nodes = typeof(GameState_Fallout4).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            fields = new Dictionary<PropertyInfo, FieldInfo[]>();
            nodeAccessors = new Dictionary<string, PropertyAccessor>();
            fieldAccessors = new Dictionary<string, FieldAccessor>();

            foreach (var node in nodes)
            {
                var nodeFields = node.PropertyType.GetFields();
                fields.Add(node, nodeFields);

                nodeAccessors.Add(node.Name, new PropertyAccessor(typeof(GameState_Fallout4), node.Name));

                foreach (var field in nodeFields)
                {
                    fieldAccessors.Add(field.Name, new FieldAccessor(node.PropertyType, field.Name));
                }
            }
            #endregion

            provider = new PipBoyStreamProvider();
            reader = new GameStateReader(provider.Connect("127.0.0.1", 27000));

        }

        public override void ResetGameState()
        {
            _game_state = new GameState_Fallout4();
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStart();
        }

        public override void UpdateTick()
        {
            try
            {
                if (!reader.NextState())
                    return;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            var localState = (_game_state as GameState_Fallout4);
            var gameState = reader.GameState;

            foreach (PropertyInfo Node in nodes)//for each node in gamestate
            {
                if (Node.PropertyType == typeof(PipBoyColorNode))
                {
                    try
                    {
                        if (((GameObject)gameState.Status).Properties.ContainsKey("EffectColor"))
                        {
                            localState.PipBoyColor.Red = (float)gameState.Status.EffectColor[0];
                            localState.PipBoyColor.Green = (float)gameState.Status.EffectColor[1];
                            localState.PipBoyColor.Blue = (float)gameState.Status.EffectColor[2];
                        }
                        else
                        {
                            //Reconnect();
                            return;
                        }
                    }
                    catch { }//ignore
                }
                else if (Node.PropertyType == typeof(SPECIALNode))
                {
                    try
                    {
                        if (((GameObject)gameState).Properties.ContainsKey("Special"))
                        {
                            localState.Special.Strength = (int)gameState.Special[0].Value;
                            localState.Special.Perception = (int)gameState.Special[1].Value;
                            localState.Special.Endurance = (int)gameState.Special[2].Value;
                            localState.Special.Charisma = (int)gameState.Special[3].Value;
                            localState.Special.Intelligence = (int)gameState.Special[4].Value;
                            localState.Special.Agility = (int)gameState.Special[5].Value;
                            localState.Special.Luck = (int)gameState.Special[6].Value;
                        }
                        else
                        {
                            //Reconnect();
                            return;
                        }
                    }
                    catch { }//ignore
                }
                else if (((GameObject)gameState).Properties.ContainsKey(Node.Name))
                {
                    GameObject gamestateProperty = reader.GameStateManager.GameObjects[gameState.Properties[Node.Name]];//obj becomes the gameobject associated with the coresponding node

                    foreach (FieldInfo Field in fields[Node])//update each field in the node
                    {
                        if (gamestateProperty.Properties.ContainsKey(Field.Name))//if the received gamestate contains the variable we're looking for
                        {
                            GameObject gamestateField = reader.GameStateManager.GameObjects[gamestateProperty.Properties[Field.Name]];//then get the gameobject with the ID we want from the gamestatemanager
                            try
                            {
                                fieldAccessors[Field.Name].Set(nodeAccessors[Node.Name].Get(localState), Convert.ChangeType(gamestateField.Primitive.ValueObject, Field.FieldType));
                            }
                            catch
                            { }//ignore
                        }
                    }
                }
            }
            base.UpdateTick();
        }

        #region Adapted from https://stackoverflow.com/questions/38528620
        class FieldAccessor
        {
            private static readonly ParameterExpression fieldParameter = Expression.Parameter(typeof(object));
            private static readonly ParameterExpression ownerParameter = Expression.Parameter(typeof(object));

            public FieldAccessor(Type type, string fieldName)
            {
                var field = type.GetField(fieldName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field == null) throw new ArgumentException();

                var fieldExpression = Expression.Field(
                    Expression.Convert(ownerParameter, type), field);

                Get = Expression.Lambda<Func<object, object>>(
                    Expression.Convert(fieldExpression, typeof(object)),
                    ownerParameter).Compile();

                Set = Expression.Lambda<Action<object, object>>(
                    Expression.Assign(fieldExpression,
                        Expression.Convert(fieldParameter, field.FieldType)),
                    ownerParameter, fieldParameter).Compile();
            }

            public Func<object, object> Get { get; }

            public Action<object, object> Set { get; }
        }

        class PropertyAccessor
        {
            private static readonly ParameterExpression propertyParameter = Expression.Parameter(typeof(object));
            private static readonly ParameterExpression ownerParameter = Expression.Parameter(typeof(object));

            public PropertyAccessor(Type type, string fieldName)
            {
                var property = type.GetProperty(fieldName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property == null) throw new ArgumentException();

                var PropertyExpression = Expression.Property(
                    Expression.Convert(ownerParameter, type), property);

                Get = Expression.Lambda<Func<object, object>>(
                    Expression.Convert(PropertyExpression, typeof(object)),
                    ownerParameter).Compile();
            }

            public Func<object, object> Get { get; }
        }
        #endregion
    }
}
