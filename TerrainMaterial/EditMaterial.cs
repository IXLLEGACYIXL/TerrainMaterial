using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.MediaFoundation;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Materials;

namespace TerrainMaterial;
[DataContract]
public class EditMaterial : SyncScript
{
    public Texture Texture { get; set; }
    public ModelComponent Material { get; set; }
    public override void Start()
    {
    }

    public override void Update()
    {
        if (this.Game.GraphicsContext.CommandList is null)
            return;
        try
        {
            Texture.GetMipMapDescription(0);
        }
        catch { return; }
        // Ensure the pixel array matches the texture size
        var width = Texture.Width;
        var height = Texture.Height;
        var x = Texture.GetDataAsImage(this.Game.GraphicsContext.CommandList);
        
        var pixelData = new Color[Texture.Width * Texture.Height];

        for (int i = 0; i < pixelData.Length; i++)
        {
            pixelData[i] = new Color(255, 0, 0, 255);
        }

        Texture.SetData<Color>(this.Game.GraphicsContext.CommandList, pixelData);
        var x2 = Material.Model.Materials[0].Material.Passes[0].Parameters.Get(MaterialKeys.DiffuseMap);
        x2.SetData(this.Game.GraphicsContext.CommandList, pixelData);
        Material.Model.Materials[0].Material.Passes[0].Parameters.Set(MaterialKeys.DiffuseMap,x2);
        Streaming.ContentStreaming.Update();
        var x3 = Streaming.Resources.Last();
        ((Texture)x3.Resource).SetData(this.Game.GraphicsContext.CommandList, pixelData);
        Streaming.Update(Game.UpdateTime);

        // Texture.Recreate();
        this.Entity.Scene = null;
    }
}
