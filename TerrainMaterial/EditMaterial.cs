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
    private int frameCount;
    public override void Update()
    {
        if (this.Game.GraphicsContext.CommandList is null)
            return;
        try
        {
            Texture.GetMipMapDescription(0);
        }
        catch { return; }
            

        if (frameCount < 250)
        {
            frameCount++;
            return;
        }


        var width = Texture.Width;
        var height = Texture.Height;
        
        var pixelData = new Color[Texture.Width * Texture.Height];

        for (int i = 0; i < pixelData.Length; i++)
        {
            pixelData[i] = new Color(255, 0, 0, 0);
        }

        var x2 = Material.Model.Materials[0].Material.Passes[0].Parameters.Get(MaterialKeys.DiffuseMap);
        var b = x2.GetData<Color>(this.Game.GraphicsContext.CommandList);
        x2.SetData(this.Game.GraphicsContext.CommandList, pixelData);
        Material.Model.Materials[0].Material.Passes[0].Parameters.Set(MaterialKeys.DiffuseMap,x2);
        this.Entity.Scene = null;
    }
}
