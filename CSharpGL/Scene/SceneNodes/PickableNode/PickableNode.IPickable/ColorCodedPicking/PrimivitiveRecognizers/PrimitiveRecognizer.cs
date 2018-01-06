﻿using System;
using System.Collections.Generic;

namespace CSharpGL
{
    internal abstract class PrimitiveRecognizer
    {
        /// <summary>
        /// 识别出以<paramref name="lastVertexId"/>结尾的图元。
        /// </summary>
        /// <param name="lastVertexId"></param>
        /// <param name="pointer"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public List<RecognizedPrimitiveInfo> Recognize(
         uint lastVertexId, IntPtr pointer, DrawElementsCmd cmd)
        {
            var lastIndexIdList = new List<RecognizedPrimitiveInfo>();
            switch (cmd.IndexBufferObject.ElementType)
            {
                case IndexBufferElementType.UByte:
                    RecognizeByte(lastVertexId, pointer, cmd, lastIndexIdList);
                    break;

                case IndexBufferElementType.UShort:
                    RecognizeUShort(lastVertexId, pointer, cmd, lastIndexIdList);
                    break;

                case IndexBufferElementType.UInt:
                    RecognizeUInt(lastVertexId, pointer, cmd, lastIndexIdList);
                    break;

                default:
                    throw new NotDealWithNewEnumItemException(typeof(IndexBufferElementType));
            }

            return lastIndexIdList;
        }

        protected abstract void RecognizeUInt(uint lastVertexId, IntPtr pointer, DrawElementsCmd cmd, List<RecognizedPrimitiveInfo> primitiveInfoList);

        protected abstract void RecognizeUShort(uint lastVertexId, IntPtr pointer, DrawElementsCmd cmd, List<RecognizedPrimitiveInfo> primitiveInfoList);

        protected abstract void RecognizeByte(uint lastVertexId, IntPtr pointer, DrawElementsCmd cmd, List<RecognizedPrimitiveInfo> primitiveInfoList);

        /// <summary>
        /// 识别出以<paramref name="lastVertexId"/>结尾的图元。
        /// <para>识别过程中要考虑排除PrimitiveRestartIndex</para>
        /// </summary>
        /// <param name="lastVertexId"></param>
        /// <param name="pointer"></param>
        /// <param name="cmd"></param>
        /// <param name="primitiveRestartIndex"></param>
        /// <returns></returns>
        public List<RecognizedPrimitiveInfo> Recognize(
            uint lastVertexId, IntPtr pointer, DrawElementsCmd cmd, uint primitiveRestartIndex)
        {
            var lastIndexIdList = new List<RecognizedPrimitiveInfo>();
            if (lastVertexId != primitiveRestartIndex)
            {
                switch (cmd.IndexBufferObject.ElementType)
                {
                    case IndexBufferElementType.UByte:
                        RecognizeByte(lastVertexId, pointer, cmd, lastIndexIdList, primitiveRestartIndex);
                        break;

                    case IndexBufferElementType.UShort:
                        RecognizeUShort(lastVertexId, pointer, cmd, lastIndexIdList, primitiveRestartIndex);
                        break;

                    case IndexBufferElementType.UInt:
                        RecognizeUInt(lastVertexId, pointer, cmd, lastIndexIdList, primitiveRestartIndex);
                        break;

                    default:
                        throw new NotDealWithNewEnumItemException(typeof(IndexBufferElementType));
                }
            }

            return lastIndexIdList;
        }

        protected abstract void RecognizeUInt(uint lastVertexId, IntPtr pointer, DrawElementsCmd cmd, List<RecognizedPrimitiveInfo> primitiveInfoList, uint primitiveRestartIndex);

        protected abstract void RecognizeUShort(uint lastVertexId, IntPtr pointer, DrawElementsCmd cmd, List<RecognizedPrimitiveInfo> primitiveInfoList, uint primitiveRestartIndex);

        protected abstract void RecognizeByte(uint lastVertexId, IntPtr pointer, DrawElementsCmd cmd, List<RecognizedPrimitiveInfo> primitiveInfoList, uint primitiveRestartIndex);
    }
}