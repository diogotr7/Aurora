using Aurora.Settings;
using Aurora.Settings.Layers;
using Aurora.Settings.Overrides;
using Aurora.Settings.Overrides.Logic;
using Aurora.Settings.Overrides.Logic.Builder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK = Aurora.Devices.DeviceKeys;

namespace Aurora.Profiles.Fallout4
{
    public class Fallout4Profile : ApplicationProfile
    {
        public Fallout4Profile() : base()
        {

        }

        public override void Reset()
        {
            base.Reset();
            Layers = new System.Collections.ObjectModel.ObservableCollection<Layer>()
            {
                new Layer("Health Points", new PercentLayerHandler()
                {
                    Properties = new PercentLayerHandlerProperties
                    {
                        _VariablePath = "PlayerInfo/CurrHP",
                        _MaxVariablePath = "PlayerInfo/MaxHP",
                        _PrimaryColor = Color.Lime,
                        _SecondaryColor = Color.Red,
                        _Sequence = new KeySequence(new[]
                        {
                             DK.F1, DK.F2, DK.F3, DK.F4, DK.F5, DK.F6, DK.F7, DK.F8, DK.F9, DK.F10, DK.F11, DK.F12
                        })
                    }
                }),
                new Layer("Action Points", new PercentLayerHandler()
                {
                    Properties = new PercentLayerHandlerProperties
                    {
                        _VariablePath = "PlayerInfo/CurrAP",
                        _MaxVariablePath = "PlayerInfo/MaxAP",
                        _PrimaryColor = Color.Yellow,
                        _SecondaryColor = Color.FromArgb(255,64,32,0),
                        _Sequence = new KeySequence(new[]
                        {
                             DK.ONE, DK.TWO, DK.THREE, DK.FOUR, DK.FIVE, DK.SIX, DK.SEVEN, DK.EIGHT, DK.NINE, DK.ZERO, DK.MINUS, DK.EQUALS
                        })
                    }
                }),
                new Layer("Pipboy Background", new SolidFillLayerHandler()
                {
                    Properties = new SolidFillLayerHandlerProperties
                    {
                        _PrimaryColor = Color.Transparent
                    }
                }
                ,
                new OverrideLogicBuilder()
                    .SetDynamicColor("_PrimaryColor", new NumberGSINumeric("PipBoyColor/Red"),
                                                      new NumberGSINumeric("PipBoyColor/Green"),
                                                      new NumberGSINumeric("PipBoyColor/Blue")
                    )
                )
            };
        }
    }
}
