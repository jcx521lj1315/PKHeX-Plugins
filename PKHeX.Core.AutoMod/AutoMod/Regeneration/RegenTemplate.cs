﻿// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Linq;
using System.Text;

namespace PKHeX.Core.AutoMod
{
    public sealed class RegenTemplate : IBattleTemplate
    {
        public int Species { get; set; }
        public int Format { get; set; }
        public string Nickname { get; set; }
        public string Gender { get; set; }
        public int HeldItem { get; set; }
        public int Ability { get; set; }
        public int Level { get; set; }
        public bool Shiny { get; set; }
        public int Friendship { get; set; }
        public int Nature { get; set; }
        public string Form { get; set; }
        public int FormIndex { get; set; }
        public int HiddenPowerType { get; set; }
        public bool CanGigantamax { get; set; }

        public int[] EVs { get; }
        public int[] IVs { get; }
        public int[] Moves { get; }

        public RegenSet Regen { get; set; } = RegenSet.Default;
        public string Text => GetSummary();

        private readonly string ParentLines;

        private RegenTemplate(IBattleTemplate set, int gen = PKX.Generation, string text = "")
        {
            Species = set.Species;
            Format = set.Format;
            Nickname = set.Nickname;
            Gender = set.Gender;
            HeldItem = set.HeldItem;
            Ability = set.Ability;
            Level = set.Level;
            Shiny = set.Shiny;
            Friendship = set.Friendship;
            Nature = set.Nature;
            Form = set.Form;
            FormIndex = set.FormIndex;
            EVs = SanitizeEVs(set.EVs, gen);
            IVs = set.IVs;
            HiddenPowerType = set.HiddenPowerType;
            Moves = set.Moves;
            CanGigantamax = set.CanGigantamax;

            ParentLines = text;
        }

        public RegenTemplate(ShowdownSet set, int gen = PKX.Generation) : this(set, gen, set.Text)
        {
            this.SanitizeForm();
            this.SanitizeBattleMoves();
            if (set.InvalidLines.Count == 0)
                return;

            var shiny = Shiny ? Core.Shiny.Always : Core.Shiny.Never;
            Regen = new RegenSet(set.InvalidLines, gen, shiny);
            Shiny = Regen.Extra.IsShiny;
        }

        public RegenTemplate(PKM pk, int gen = PKX.Generation) : this(new ShowdownSet(pk), gen)
        {
            this.FixGender(pk.PersonalInfo);
            Regen = new RegenSet(pk);
            Shiny = Regen.Extra.IsShiny;
        }

        private static int[] SanitizeEVs(int[] evs, int gen)
        {
            var copy = (int[])evs.Clone();
            int maxEV = gen >= 6 ? 252 : gen >= 3 ? 255 : 65535;
            for (int i = 0; i < evs.Length; i++)
            {
                if (copy[i] > maxEV)
                    copy[i] = maxEV;
            }
            return copy;
        }

        private string GetSummary()
        {
            var sb = new StringBuilder();
            var text = ParentLines;
            var split = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            var reordered = split.Where(z => !IsIgnored(z)).GroupBy(z => z.StartsWith("- ")).ToArray();
            sb.AppendLine(string.Join(Environment.NewLine, reordered[0])); // Not Moves
            sb.AppendLine(Regen.GetSummary());
            if (reordered.Length > 1)
                sb.AppendLine(string.Join(Environment.NewLine, reordered[1])); // Moves
            return sb.ToString();
        }

        private static bool IsIgnored(string s)
        {
            return s.StartsWith("Shiny: ");
        }
    }
}
