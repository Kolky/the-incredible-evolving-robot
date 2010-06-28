using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Handlers
{
    public class TextHandler
    {
        public class TextObject
        {
            #region Properties
            public FontType FontType { get; set; }
            public Color Color { get; set; }
            public String Text { get; set; }
            public Vector2 Position { get; set; }
            public float Rotation { get; set; }
            public Vector2 Origin { get; set; }
            public float Scale { get; set; }
            public SpriteEffects Effects { get; set; }
            public Boolean CenterX { get; set; }
            public Boolean CenterY { get; set; }
            #endregion

            public TextObject(FontType fontType, String text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, Boolean centerX, Boolean centerY)
            {
                FontType = fontType;

                Text = text;
                Position = position;
                Color = color;

                Rotation = rotation;
                Origin = origin;
                Scale = scale;
                Effects = effects;

                CenterX = centerX;
                CenterY = centerY;
            }
        }

        #region Properties
        public enum FontType
        {
            DEFAULT_FONT,
            TEXTURE_FONT
        }

        private SpriteBatch fontBatch;
        private SpriteFont font;
        private Texture2D fontTexture;

        private Hashtable textToWrite;

        /// <summary>
        /// Font height
        /// </summary>
        private const int FontHeight = 36;

        /// <summary>
        /// Substract this value from the y postion when rendering.
        /// Most letters start below the CharRects, this fixes that issue.
        /// </summary>
        private const int SubRenderHeight = 5;

        /// <summary>
        /// Char rectangles, goes from space (32) to ~ (126).
        /// Height is not used (always the same), instead we save the actual
        /// used width for rendering in the height value!
        /// Then we also got 4 extra rects for the XBox Buttons: A, B, X, Y
        /// </summary>
        private static Rectangle[] CharRects = new Rectangle[126 - 32 + 1]
        {
            new Rectangle(0, 0, 1, 8), // space
            new Rectangle(1, 0, 11, 10), // !
            new Rectangle(12, 0, 14, 13), // "
            new Rectangle(26, 0, 20, 18), // #
            new Rectangle(46, 0, 20, 18), // $
            new Rectangle(66, 0, 24, 22), // %
            new Rectangle(90, 0, 25, 23), // &
            new Rectangle(115, 0, 8, 7), // '
            new Rectangle(124, 0, 10, 9), // (
            new Rectangle(136, 0, 10, 9), // )
            new Rectangle(146, 0, 20, 18), // *
            new Rectangle(166, 0, 20, 18), // +
            new Rectangle(186, 0, 10, 8), // ,
            new Rectangle(196, 0, 10, 9), // -
            new Rectangle(207, 0, 10, 8), // .
            new Rectangle(217, 0, 18, 16), // /
            new Rectangle(235, 0, 20, 19), // 0

            new Rectangle(0, 36, 20, 18), // 1
            new Rectangle(20, 36, 20, 18), // 2
            new Rectangle(40, 36, 20, 18), // 3
            new Rectangle(60, 36, 21, 19), // 4
            new Rectangle(81, 36, 20, 18), // 5
            new Rectangle(101, 36, 20, 18), // 6
            new Rectangle(121, 36, 20, 18), // 7
            new Rectangle(141, 36, 20, 18), // 8
            new Rectangle(161, 36, 20, 18), // 9
            new Rectangle(181, 36, 10, 8), // :
            new Rectangle(191, 36, 10, 8), // ;
            new Rectangle(201, 36, 20, 18), // <
            new Rectangle(221, 36, 20, 18), // =

            new Rectangle(0, 72, 20, 18), // >
            new Rectangle(20, 72, 19, 17), // ?
            new Rectangle(39, 72, 26, 24), // @
            new Rectangle(65, 72, 22, 20), // A
            new Rectangle(87, 72, 22, 20), // B
            new Rectangle(109, 72, 22, 20), // C
            new Rectangle(131, 72, 23, 21), // D
            new Rectangle(154, 72, 20, 18), // E
            new Rectangle(174, 72, 19, 17), // F
            new Rectangle(193, 72, 23, 21), // G
            new Rectangle(216, 72, 23, 21), // H
            new Rectangle(239, 72, 11, 10), // I

            new Rectangle(0, 108, 15, 13), // J
            new Rectangle(15, 108, 22, 20), // K
            new Rectangle(37, 108, 19, 17), // L
            new Rectangle(56, 108, 29, 26), // M
            new Rectangle(85, 108, 23, 21), // N
            new Rectangle(108, 108, 24, 22), // O
            new Rectangle(132, 108, 22, 20), // P
            new Rectangle(154, 108, 24, 22), // Q
            new Rectangle(178, 108, 24, 22), // R
            new Rectangle(202, 108, 21, 19), // S
            new Rectangle(223, 108, 17, 15), // T

            new Rectangle(0, 144, 22, 20), // U
            new Rectangle(22, 144, 22, 20), // V
            new Rectangle(44, 144, 30, 28), // W
            new Rectangle(74, 144, 22, 20), // X
            new Rectangle(96, 144, 20, 18), // Y
            new Rectangle(116, 144, 20, 18), // Z
            new Rectangle(136, 144, 10, 9), // [
            new Rectangle(146, 144, 18, 16), // \
            new Rectangle(167, 144, 10, 9), // ]
            new Rectangle(177, 144, 17, 16), // ^
            new Rectangle(194, 144, 17, 16), // _
            new Rectangle(211, 144, 17, 16), // `
            new Rectangle(228, 144, 20, 18), // a

            new Rectangle(0, 180, 20, 18), // b
            new Rectangle(20, 180, 18, 16), // c
            new Rectangle(38, 180, 20, 18), // d
            new Rectangle(58, 180, 20, 18), // e
            new Rectangle(79, 180, 14, 12), // f
            new Rectangle(93, 180, 20, 18), // g
            new Rectangle(114, 180, 19, 18), // h
            new Rectangle(133, 180, 11, 10), // i
            new Rectangle(145, 180, 11, 10), // j
            new Rectangle(156, 180, 20, 18), // k
            new Rectangle(176, 180, 11, 9), // l
            new Rectangle(187, 180, 29, 27), // m
            new Rectangle(216, 180, 20, 18), // n
            new Rectangle(236, 180, 20, 19), // o

            new Rectangle(0, 216, 20, 18), // p
            new Rectangle(20, 216, 20, 18), // q
            new Rectangle(40, 216, 13, 12), // r
            new Rectangle(53, 216, 17, 16), // s
            new Rectangle(70, 216, 14, 11), // t
            new Rectangle(84, 216, 19, 18), // u
            new Rectangle(104, 216, 17, 16), // v
            new Rectangle(122, 216, 25, 23), // w
            new Rectangle(148, 216, 19, 17), // x
            new Rectangle(168, 216, 18, 16), // y
            new Rectangle(186, 216, 16, 15), // z
            new Rectangle(203, 216, 10, 9), // {
            new Rectangle(214, 216, 12, 11), // |
            new Rectangle(227, 216, 10, 9), // }
            new Rectangle(237, 216, 18, 17), // ~
        };
        #endregion

        public TextHandler()
        {
            textToWrite = new Hashtable();
        }

        public void Initialize()
        {
            fontBatch = new SpriteBatch(TierGame.Device);
            font = TierGame.Instance.Content.Load<SpriteFont>("Content\\Fonts\\Arial");
            fontTexture = TierGame.Instance.Content.Load<Texture2D>("Content\\Fonts\\GameFont");
        }

        #region AddItem
        public void AddItem(String key, String text, Vector2 position)
        {
            AddItem(key, text, position, Color.White);
        }
        public void AddItem(String key, String text, Vector2 position, Color color)
        {
            AddItem(key, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None);
        }
        public void AddItem(String key, String text, Vector2 position, Color color, Boolean centerX, Boolean centerY)
        {
            AddItem(key, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, centerX, centerY);
        }
        public void AddItem(String key, String text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects)
        {
            AddItem(key, text, position, color, rotation, origin, scale, effects, false, false);
        }
        public void AddItem(String key, String text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, Boolean centerX, Boolean centerY)
        {
            TextObject obj = new TextObject(FontType.TEXTURE_FONT, text, position, color, rotation, origin, scale, effects, centerX, centerY);

            if (!textToWrite.Contains(key))
                textToWrite.Add(key, obj);
        }
        #endregion

        #region RemoveItem
        public void RemoveItem(String key)
        {
            if (textToWrite.Contains(key))
                textToWrite.Remove(key);
        }
        #endregion

        #region ChangeItem
        public void ChangeText(String key, String text)
        {
            if (textToWrite.Contains(key))
            {
                TextObject obj = (TextObject)textToWrite[key];
                obj.Text = text;
                textToWrite[key] = obj;
            }
        }
        public void ChangePosition(String key, Vector2 position)
        {
            if (textToWrite.Contains(key))
            {
                TextObject obj = (TextObject)textToWrite[key];
                obj.Position = position;
                textToWrite[key] = obj;
            }
        }
        public void ChangeColor(String key, Color color)
        {
            if (textToWrite.Contains(key))
            {
                TextObject obj = (TextObject)textToWrite[key];
                obj.Color = color;
                textToWrite[key] = obj;
            }
        }
        public void ChangeRotation(String key, float rotation)
        {
            if (textToWrite.Contains(key))
            {
                TextObject obj = (TextObject)textToWrite[key];
                obj.Rotation = rotation;
                textToWrite[key] = obj;
            }
        }
        public void ChangeOrigin(String key, Vector2 origin)
        {
            if (textToWrite.Contains(key))
            {
                TextObject obj = (TextObject)textToWrite[key];
                obj.Origin = origin;
                textToWrite[key] = obj;
            }
        }
        public void ChangeScale(String key, float scale)
        {
            if (textToWrite.Contains(key))
            {
                TextObject obj = (TextObject)textToWrite[key];
                obj.Scale = scale;
                textToWrite[key] = obj;
            }
        }
        public void ChangeEffects(String key, SpriteEffects effects)
        {
            if (textToWrite.Contains(key))
            {
                TextObject obj = (TextObject)textToWrite[key];
                obj.Effects = effects;
                textToWrite[key] = obj;
            }
        }
        #endregion

        #region GetTextWidth
        public Vector2 GetTextWidth(String key)
        {
            if (textToWrite.Contains(key))
            {
                if (((TextObject)textToWrite[key]).FontType == FontType.DEFAULT_FONT)
                    return font.MeasureString(((TextObject)textToWrite[key]).Text);
                else if (((TextObject)this.textToWrite[key]).FontType == FontType.TEXTURE_FONT)
                {
                    int width = 0, height = 0;
                    char[] chars = ((TextObject)textToWrite[key]).Text.ToCharArray();
                    for (int num = 0; num < chars.Length; num++)
                    {
                        int charNum = (int)chars[num];
                        if (charNum >= 32 && charNum - 32 < CharRects.Length)
                            width += CharRects[charNum - 32].Height;
                    }
                    return new Vector2(width, height);
                }
                else
                    return Vector2.Zero;
            }
            else
                return Vector2.Zero;
        }
        #endregion

        #region GetTextObject
        public TextObject GetTextObject(String key)
        {
            if (textToWrite.Contains(key))
                return (TextObject)textToWrite[key];
            else
                return null;
        }
        #endregion

        #region Draw
        public void Draw(GameTime gameTime)
        {
            if (textToWrite.Count > 0)
            {
                fontBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);

                IDictionaryEnumerator iter = textToWrite.GetEnumerator();
                while (iter.MoveNext())
                {
                    if (((TextObject)iter.Value).CenterX || ((TextObject)iter.Value).CenterY)
                        ((TextObject)iter.Value).Origin = new Vector2((((TextObject)iter.Value).CenterX ? (GetTextWidth((String)iter.Key).X * 0.5f) : ((TextObject)iter.Value).Origin.X),
                          (((TextObject)iter.Value).CenterY ? (GetTextWidth((String)iter.Key).Y * 0.5f) : ((TextObject)iter.Value).Origin.Y));

                    if (((TextObject)iter.Value).FontType == FontType.DEFAULT_FONT)
                    {
                        // Draw using normal Arial Font
                        fontBatch.DrawString(font, ((TextObject)iter.Value).Text, ((TextObject)iter.Value).Position, ((TextObject)iter.Value).Color, ((TextObject)iter.Value).Rotation, ((TextObject)iter.Value).Origin, ((TextObject)iter.Value).Scale, ((TextObject)iter.Value).Effects, 0f);
                    }
                    else if (((TextObject)iter.Value).FontType == FontType.TEXTURE_FONT)
                    {
                        // Draw using texture Font
                        char[] chars = ((TextObject)iter.Value).Text.ToCharArray();
                        float posX = ((TextObject)iter.Value).Position.X;
                        float posY = ((TextObject)iter.Value).Position.Y;

                        for (int num = 0; num < chars.Length; num++)
                        {
                            int charNum = (int)chars[num];
                            if (charNum >= 32 && charNum - 32 < CharRects.Length)
                            {
                                Rectangle rect = CharRects[charNum - 32];

                                // Reduce height to prevent overlapping pixels
                                rect.Y += 1;
                                rect.Height = FontHeight;
                                Rectangle destRect = new Rectangle((int)posX, (int)posY - TextHandler.SubRenderHeight, rect.Width, rect.Height);

                                destRect.Width = (int)Math.Round(destRect.Width * ((TextObject)iter.Value).Scale);
                                destRect.Height = (int)Math.Round(destRect.Height * ((TextObject)iter.Value).Scale);

                                // Since we want upscaling, we use the modified destRect
                                fontBatch.Draw(fontTexture, destRect, rect, ((TextObject)iter.Value).Color, ((TextObject)iter.Value).Rotation, ((TextObject)iter.Value).Origin, ((TextObject)iter.Value).Effects, 0f);

                                // Increase x pos by width we use for this character
                                int charWidth = CharRects[charNum - 32].Height;
                                posX += (int)Math.Round(charWidth * ((TextObject)iter.Value).Scale);
                            }
                        }
                    }
                }
                fontBatch.End();
            }
        }
        #endregion
    }
}