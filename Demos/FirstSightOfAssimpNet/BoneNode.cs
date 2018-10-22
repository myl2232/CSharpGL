﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpGL;

namespace FirstSightOfAssimpNet
{
    partial class BoneNode : ModernNode, IRenderable
    {
        public static BoneNode Create(BoneModel model)
        {
            var vs = new VertexShader(vertexCode);
            var fs = new FragmentShader(fragmentCode);
            var array = new ShaderArray(vs, fs);
            var map = new AttributeMap();
            map.Add("inPosition", BoneModel.strPosition);
            map.Add("inTexCoord", BoneModel.strTexCoord);
            map.Add("inBoneIDs", BoneModel.strBoneIDs);
            map.Add("inWeights", BoneModel.strWeights);
            var builder = new RenderMethodBuilder(array, map);//, new PolygonModeSwitch(PolygonMode.Line));
            var node = new BoneNode(model, builder);
            node.Initialize();

            return node;
        }

        private BoneNode(IBufferSource model, params RenderMethodBuilder[] builders) : base(model, builders) { }


    }
}
