﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KEngine.API.Compress
{
    /// <summary>
    /// 压缩管理器
    /// </summary>
    public interface ICompressManager //: ISingleManager<ICompress>
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="bytes">需要压缩的字节流</param>
        /// <param name="name">使用的压缩解压缩名字</param>
        /// <returns>压缩后的结果</returns>
        byte[] Compress(byte[] bytes, string name = null);

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="bytes">需要解压缩的字节流</param>
        /// <param name="name">使用的压缩解压缩名字</param>
        /// <returns>解压缩的结果</returns>
        byte[] Decomporess(byte[] bytes, string name = null);
    }
}