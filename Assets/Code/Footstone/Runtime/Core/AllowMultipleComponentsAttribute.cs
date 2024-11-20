// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System;

namespace Lost.Runtime.Footstone.Core
{
    //使用这个标签的必须是主Key
    //TODO:增加检查 使用该标签的类必须是主Key
    [AttributeUsage(AttributeTargets.Class)]
    public class AllowMultipleComponentsAttribute : EntityComponentAttributeBase
    {
    }
}
