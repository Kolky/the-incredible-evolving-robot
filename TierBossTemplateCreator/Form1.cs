using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace TierBossTemplateCreator
{
  public partial class Form1 : Form
  {
    private GraphicsDevice graphicsDeviceMain;
    private GraphicsDevice graphicsDeviceSelect;

    public Form1()
    {
      InitializeComponent();

      PresentationParameters pp = new PresentationParameters();
      pp.IsFullScreen = false;

      graphicsDeviceSelect = new GraphicsDevice(
        GraphicsAdapter.DefaultAdapter,
        DeviceType.Reference,
        this.pnlSelect.Handle,
        pp);

      graphicsDeviceMain = new GraphicsDevice(
        GraphicsAdapter.DefaultAdapter,
        DeviceType.Reference,
        this.pnlMain.Handle,
        pp);
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
      graphicsDeviceMain.Clear(
        Microsoft.Xna.Framework.Graphics.Color.CornflowerBlue);
      graphicsDeviceMain.Present();

      graphicsDeviceSelect.Clear(
        Microsoft.Xna.Framework.Graphics.Color.Pink);
      graphicsDeviceSelect.Present();
    }
  }
}
