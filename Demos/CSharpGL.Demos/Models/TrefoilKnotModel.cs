﻿using System;

namespace CSharpGL.Demos
{
    /// <summary>
    /// Trefoil knot model.
    /// </summary>
    internal class TrefoilKnotModel : IBufferable
    {
        private double interval;

        private int GetUCount(double interval)
        {
            int uCount = (int)(Math.PI * 2 / interval);
            return uCount;
        }

        public TrefoilKnotModel(double interval = 0.02)
        {
            this.interval = interval;
            bool initialized = false;
            vec3 max = new vec3();
            vec3 min = new vec3();
            int uCount = GetUCount(interval);
            for (int uIndex = 0; uIndex < uCount; uIndex++)
            {
                double t = Math.PI * 2 * uIndex / uCount;
                var position = GetPosition(t);

                if (!initialized)
                {
                    max = position;
                    min = position;
                    initialized = true;
                }
                else
                {
                    position.UpdateMax(ref max);
                    position.UpdateMin(ref min);
                }
            }
            this.Lengths = max - min;
        }

        public const string strPosition = "position";
        private VertexAttributeBufferPtr positionBufferPtr;

        public const string strTexCoord = "texCoord";
        private VertexAttributeBufferPtr colorBufferPtr;

        private IndexBufferPtr indexBufferPtr = null;

        /// <summary>
        /// 获取指定的顶点属性缓存。
        /// <para>Gets specified vertex buffer object.</para>
        /// </summary>
        /// <param name="bufferName">buffer name(Gets this name from 'strPosition' etc.</param>
        /// <param name="varNameInShader">name in vertex shader like `in vec3 in_Position;`.</param>
        /// <returns>Vertex Buffer Object.</returns>
        public VertexAttributeBufferPtr GetVertexAttributeBufferPtr(string bufferName, string varNameInShader)
        {
            if (bufferName == strPosition)
            {
                if (this.positionBufferPtr == null)
                {
                    this.positionBufferPtr = GetPositionBufferPtr(varNameInShader);
                }
                return this.positionBufferPtr;
            }
            else if (bufferName == strTexCoord)
            {
                if (this.colorBufferPtr == null)
                {
                    this.colorBufferPtr = GetTexCoordBufferPtr(varNameInShader);
                }
                return this.colorBufferPtr;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private VertexAttributeBufferPtr GetTexCoordBufferPtr(string varNameInShader)
        {
            int uCount = GetUCount(interval);
            int length = uCount;
            VertexAttributeBufferPtr bufferPtr = VertexAttributeBufferPtr.Create(typeof(float), length, VertexAttributeConfig.Float, BufferUsage.StaticDraw, varNameInShader);
            unsafe
            {
                IntPtr pointer = bufferPtr.MapBuffer(MapBufferAccess.WriteOnly);
                int index = 0;
                var array = (float*)pointer;
                for (int uIndex = 0; uIndex < uCount; uIndex++)
                {
                    array[index++] = (float)uIndex / (float)uCount;
                }
                bufferPtr.UnmapBuffer();
            }

            return bufferPtr;
        }

        private VertexAttributeBufferPtr GetPositionBufferPtr(string varNameInShader)
        {
            int uCount = GetUCount(interval);
            int length = uCount;
            VertexAttributeBufferPtr bufferPtr = VertexAttributeBufferPtr.Create(typeof(vec3), length, VertexAttributeConfig.Vec3, BufferUsage.StaticDraw, varNameInShader);
            bool initialized = false;
            vec3 max = new vec3();
            vec3 min = new vec3();
            unsafe
            {
                IntPtr pointer = bufferPtr.MapBuffer(MapBufferAccess.WriteOnly);
                int index = 0;
                var array = (vec3*)pointer;
                for (int uIndex = 0; uIndex < uCount; uIndex++)
                {
                    double t = Math.PI * 2 * uIndex / uCount;
                    var position = GetPosition(t);

                    if (!initialized)
                    {
                        max = position;
                        min = position;
                        initialized = true;
                    }
                    else
                    {
                        position.UpdateMax(ref max);
                        position.UpdateMin(ref min);
                    }
                    array[index++] = position;
                }
                //this.Lengths = max - min;
                vec3 worldPosition = max / 2.0f + min / 2.0f;
                for (int i = 0; i < index; i++)
                {
                    array[i] = array[i] - worldPosition;
                }
                bufferPtr.UnmapBuffer();
            }

            return bufferPtr;
        }

        public vec3 Lengths { get; private set; }

        public vec3 GetPosition(double t)
        {
            double sinT = Math.Sin(t), cosT = Math.Cos(t);
            double sin2T = Math.Sin(2 * t), cos2T = Math.Cos(2 * t);
            double sin3T = Math.Sin(3 * t);//, cos3T = Math.Cos(3 * t);
            double x = sinT + 2 * sin2T;
            double y = cosT - 2 * cos2T;
            double z = -sin3T;

            return new vec3((float)x, (float)y, (float)z);
        }

        public IndexBufferPtr GetIndexBufferPtr()
        {
            if (this.indexBufferPtr == null)
            {
                int uCount = GetUCount(interval);
                ZeroIndexBufferPtr bufferPtr = ZeroIndexBufferPtr.Create(DrawMode.Points, 0, uCount);
                this.indexBufferPtr = bufferPtr;
            }

            return this.indexBufferPtr;
        }

        /// <summary>
        /// Uses <see cref="ZeroIndexBuffer"/> or <see cref="OneIndexBuffer"/>.
        /// </summary>
        /// <returns></returns>
        public bool UsesZeroIndexBuffer() { return true; }
    }
}