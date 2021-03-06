﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SassyStudio.Compiler.Parsing
{
    public class HexColorValue : SimplexItem
    {
        public TokenItem Hash { get; protected set; }
        public TokenItem Color { get; protected set; }

        public override bool Parse(IItemFactory itemFactory, ITextProvider text, ITokenStream stream)
        {
            if (stream.Current.Type == TokenType.Hash)
            {
                Hash = Children.AddCurrentAndAdvance(stream, SassClassifierType.Default);
                Color = Children.AddCurrentAndAdvance(stream, SassClassifierType.Default);
            }

            return Children.Count > 0;
        }
    }
}
