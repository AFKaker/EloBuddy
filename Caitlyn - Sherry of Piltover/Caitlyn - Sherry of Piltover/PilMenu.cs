using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace Caitlyn___Sherry_of_Piltover
{
    internal class PilMenu
    {
        private static Menu Caitlyn, ComboMenu, ComboLogic, Draw, Misc, HarassMenu, Lane, Jungle;
        public static readonly AIHeroClient Enemy = ObjectManager.Player;



        internal static void Load()
        {
            Caitlyn = MainMenu.AddMenu("Caitlyn", "Caitlyn");
            ComboLogic = Caitlyn.AddSubMenu("Logic Combo");
            ComboLogic.Add("Combo1", new ComboBox("Combos Logic", 0, "Q - E - W - R", "E - Q - W", "W - E - Q"));
            //
            ComboMenu = Caitlyn.AddSubMenu("Combo");

            ComboMenu.AddLabel("Config Q Spell");
            ComboMenu.Add("CQ", new CheckBox("Utility Q In Combo"));
            ComboMenu.Add("CQS", new Slider("Hit Chance Q Spell", 45));
            ComboMenu.AddLabel("Config W Spell");
            ComboMenu.Add("CW", new CheckBox("Utility W In Combo"));
            ComboMenu.Add("CWS", new Slider("Hit Chance W Spell", 65));
            ComboMenu.AddLabel("Config E Spell");
            ComboMenu.Add("CE", new CheckBox("Utility E In Combo"));
            ComboMenu.Add("CES", new Slider("Hit Chance E Spell", 50));
            ComboMenu.AddLabel("Config R Spell");
            ComboMenu.Add("CR", new CheckBox("Utility R In Combo", false));
            ComboMenu.Add("AceR", new CheckBox("Utility R Ace Enemy"));
            //Harass
            HarassMenu = Caitlyn.AddSubMenu("Harass");
            HarassMenu.AddLabel("Config Harass Spell Q");
            HarassMenu.Add("HQ", new CheckBox("Harass Q"));
            HarassMenu.Add("HQS", new Slider("Min Mana", 60));
            HarassMenu.Add("HR", new CheckBox("Harass R", false));
            //LaneClear
            Lane = Caitlyn.AddSubMenu("Lane");
            Lane.Add("LQ", new CheckBox("Use Q In LaneClear"));
            Lane.Add("LQS", new Slider("Mini Man", 60));
            Lane.AddLabel("Min Minions");
            Lane.Add("Minion", new Slider("Minions", 1, 3, 5));
            //JungleClear
            Jungle = Caitlyn.AddSubMenu("Jungle");
            Jungle.Add("JQ", new CheckBox("Use Q JungleClear"));
            Jungle.Add("JQS", new Slider("Mini Man", 60));
            Misc = Caitlyn.AddSubMenu("Misc");
            Misc.Add("IntW", new CheckBox("Inter W Enemy"));
            Misc.Add("IntE", new CheckBox("Inter E Enemy"));
            Misc.AddLabel("Config R");
            Misc.Add("Rc", new CheckBox("Logic R"));
            Misc.Add("ConfigR", new CheckBox("Uti Ace Enemy"));
            Misc.Add("Auto", new CheckBox("Auto Range R Automatic", false));
            Misc.Add("Autoconju", new KeyBind("Forced R", false, KeyBind.BindTypes.HoldActive, 'A'));
            Draw = Caitlyn.AddSubMenu("Draw");
            Draw.Add("Draw", new CheckBox("Enable Drawings"));
            Draw.AddLabel("Spell Q");
            Draw.Add("DQ", new CheckBox("Draw Q Spell"));
            Draw.AddLabel("Spell W");
            Draw.Add("DW", new CheckBox("Draw W Spell", false));
            Draw.AddLabel("Spell E");
            Draw.Add("DE", new CheckBox("Draw E Spell", false));
            Draw.AddLabel("Spell R");
            Draw.Add("DR", new CheckBox("Draw R Spell"));
            Draw.AddLabel("Config Damage");
            Draw.Add("damage", new CheckBox("Damager"));

            Drawing.OnDraw += OnDraw;
            Game.OnTick += OnTick;
            Gapcloser.OnGapcloser += AntiGapCloser;
            Orbwalker.OnPostAttack += Attack;


        }

        private static void Attack(AttackableUnit target, EventArgs args)
        {

        }

        private static void AntiGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Misc["IntE"].Cast<CheckBox>().CurrentValue && sender.IsEnemy && sender.IsValidTarget(Spells.E.Range) &&
                e.End.Distance(Enemy) <= 250)
            {
                Spells.E.Cast(e.End);
            }
            if (Misc["IntW"].Cast<CheckBox>().CurrentValue && sender.IsEnemy && sender.IsValidTarget(Spells.W.Range) &&
                e.End.Distance(Enemy) <= 250)
            {
                Spells.W.Cast(e.End);
            }
        }

        private static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LanClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JunClear();
            }
            if (Misc["Auto"].Cast<CheckBox>().CurrentValue)
            {
                RUti();
            }
        }

        private static void RUti()
        {
            if (Misc["Autoconju"].Cast<KeyBind>().CurrentValue)
            {

                var rkILtarget = TargetSelector.GetTarget(Spells.R.Range, DamageType.Physical);
                if (rkILtarget == null) return;
                {
                    Spells.R.Cast(rkILtarget);

                    var predictedHealth = Prediction.Health.GetPrediction(rkILtarget, Spells.R.CastDelay + Game.Ping);
                }
            }
        }
        private static void Harass()
        {
            if (HarassMenu["HQ"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);

                var Qcait = Spells.Q.GetPrediction(target);
                if (target.IsValidTarget(Spells.Q.Range) && Spells.Q.IsReady() && Qcait.HitChance >= HitChance.Medium)
                {
                    Spells.Q.Cast(target);
                }
            }
            if (HarassMenu["HR"].Cast<CheckBox>().CurrentValue)
            {
                var target = TargetSelector.GetTarget(Spells.R.Range, DamageType.Physical);
                if (!target.IsValidTarget(Spells.R.Range) || target == null)
                {
                    return;

                    {
                        Spells.R.Cast(target);
                    }
                }
            }
        }

        private static void JunClear()
        {
            var playerMana = Player.Instance.ManaPercent;
            if (Jungle["JQ"].Cast<CheckBox>().CurrentValue && playerMana > Jungle["JQS"].Cast<Slider>().CurrentValue)
            {
                bool lastQ = false;
                var monsters = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition, Spells.Q.Range).OrderBy(h => h.Health);
                {

                    if (monsters.Any() && !lastQ)
                    {
                        var getHealthyCs = monsters.GetEnumerator();
                        while (getHealthyCs.MoveNext())
                        {
                            Spells.Q.Cast(Spells.Q.GetPrediction(monsters.Last()).CastPosition);
                        }
                    }
                }
            }
        }
        private static void LanClear()
        {
            var playerMana = Player.Instance.ManaPercent;

            if (Lane["LQ"].Cast<CheckBox>().CurrentValue && playerMana > Lane["LQS"].Cast<Slider>().CurrentValue)
            {
                bool lastQ = false;
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition, Spells.Q.Range).OrderBy(h => h.Health);
                {

                    if (minions.Any() && !lastQ)
                    {
                        var getHealthyCs = minions.GetEnumerator();
                        while (getHealthyCs.MoveNext())
                        {
                            Spells.Q.Cast(Spells.Q.GetPrediction(minions.Last()).CastPosition);
                        }
                    }
                }
            }
        }

        private static void Combo()
        {
            if (ComboLogic["Combo1"].Cast<ComboBox>().CurrentValue == 0)
            {
                if (ComboMenu["CQ"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);

                    var Qcait = Spells.Q.GetPrediction(target);
                    if (target.IsValidTarget(Spells.Q.Range) && Spells.Q.IsReady() && Qcait.HitChance >= HitChance.High)
                    {
                        Spells.Q.Cast(target);
                    }

                }
                if (ComboMenu["CE"].Cast<CheckBox>().CurrentValue)
                {
                    var etarget = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
                    var Ecait = Spells.E.GetPrediction(etarget);
                    if (etarget.IsValidTarget(Spells.E.Range) && Spells.E.IsReady() && Ecait.HitChance >= HitChance.High)
                    {
                        Spells.E.Cast(etarget);
                    }
                }
                if (ComboMenu["CW"].Cast<CheckBox>().CurrentValue)
                {
                    var wtarget = TargetSelector.GetTarget(Spells.W.Range, DamageType.Physical);
                    var Wcait = Spells.W.GetPrediction(wtarget);
                    if (wtarget.IsValidTarget(Spells.W.Range) && Spells.W.IsReady() && Wcait.HitChance >= HitChance.High)
                    {
                        Spells.W.Cast(wtarget);
                    }
                }

                if (ComboMenu["CR"].Cast<CheckBox>().CurrentValue)
                {
                    var rkILtarget = TargetSelector.GetTarget(Spells.R.Range, DamageType.Physical);
                    if (rkILtarget == null) return;
                    {
                        Spells.R.Cast(rkILtarget);
                    }
                }
            }

            if (ComboLogic["Combo1"].Cast<ComboBox>().CurrentValue == 1)
            {
                if (ComboMenu["CE"].Cast<CheckBox>().CurrentValue)
                {
                    var etarget = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
                    var Ecait = Spells.E.GetPrediction(etarget);
                    if (etarget.IsValidTarget(Spells.E.Range) && Spells.E.IsReady() && Ecait.HitChance >= HitChance.High)
                    {
                        Spells.E.Cast(etarget);
                    }
                }
                if (ComboMenu["CQ"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);

                    var Qcait = Spells.Q.GetPrediction(target);
                    if (target.IsValidTarget(Spells.Q.Range) && Spells.Q.IsReady() && Qcait.HitChance >= HitChance.High)
                    {
                        Spells.Q.Cast(target);
                    }

                }
                if (ComboMenu["CW"].Cast<CheckBox>().CurrentValue)
                {
                    var wtarget = TargetSelector.GetTarget(Spells.W.Range, DamageType.Physical);
                    var Wcait = Spells.W.GetPrediction(wtarget);
                    if (wtarget.IsValidTarget(Spells.W.Range) && Spells.W.IsReady() && Wcait.HitChance >= HitChance.High)
                    {
                        Spells.W.Cast(wtarget);
                    }
                }
                if (ComboMenu["CR"].Cast<CheckBox>().CurrentValue)
                {
                    var rtarget = TargetSelector.GetTarget(Spells.R.Range, DamageType.Physical);
                    if (rtarget == null) return;
                    {
                        Spells.R.Cast(rtarget);
                    }
                }
            }
            if (ComboLogic["Combo1"].Cast<ComboBox>().CurrentValue == 2)
            {
                if (ComboMenu["CW"].Cast<CheckBox>().CurrentValue)
                {
                    var wtarget = TargetSelector.GetTarget(Spells.W.Range, DamageType.Physical);
                    var Wcait = Spells.W.GetPrediction(wtarget);
                    if (wtarget.IsValidTarget(Spells.W.Range) && Spells.W.IsReady() && Wcait.HitChance >= HitChance.High)
                    {
                        Spells.W.Cast(wtarget);
                    }
                }
                if (ComboMenu["CQ"].Cast<CheckBox>().CurrentValue)
                {
                    var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);

                    var Qcait = Spells.Q.GetPrediction(target);
                    if (target.IsValidTarget(Spells.Q.Range) && Spells.Q.IsReady() && Qcait.HitChance >= HitChance.High)
                    {
                        Spells.Q.Cast(target);
                    }

                }
                if (ComboMenu["CE"].Cast<CheckBox>().CurrentValue)
                {
                    var etarget = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
                    var Ecait = Spells.E.GetPrediction(etarget);
                    if (etarget.IsValidTarget(Spells.E.Range) && Spells.E.IsReady() && Ecait.HitChance >= HitChance.High)
                    {
                        Spells.E.Cast(etarget);
                    }
                }
            }
        }




        private static void OnDraw(EventArgs args)
        {
            if (!Draw["Draw"].Cast<CheckBox>().CurrentValue) return;

            if (Draw["DQ"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady()) Circle.Draw(Color.LightBlue, Spells.Q.Range, Player.Instance.Position);

            if (Draw["DW"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady()) Circle.Draw(Color.RoyalBlue, Spells.W.Range, Player.Instance.Position);

            if (Draw["DE"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady()) Circle.Draw(Color.LightPink, Spells.E.Range, Player.Instance.Position);

            if (Draw["DR"].Cast<CheckBox>().CurrentValue && Spells.R.IsReady()) Circle.Draw(Color.LightSkyBlue, Spells.R.Range, Player.Instance.Position);
        }
    }
}
