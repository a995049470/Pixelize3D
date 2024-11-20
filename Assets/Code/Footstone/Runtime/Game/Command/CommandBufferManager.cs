using System;
using System.Collections.Generic;
using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    public class CommandBufferManager : IUpdateable
    {
        private IServiceRegistry service;

        private Stack<CommandBuffer> commandBufferPool;
        public CommandBuffer FrameEndBuffer { get; private set; }

        public CommandBufferManager(IServiceRegistry _service)
        {
            service = _service;
            commandBufferPool = new Stack<CommandBuffer>(4);
            FrameEndBuffer = new CommandBuffer(_service);
        }

        public bool Enabled => true;
        public int UpdateOrder => 200;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        
        public void Update(GameTime gameTime)
        {
            FrameEndBuffer.Execute();
        }

        public CommandBuffer Get()
        {
            CommandBuffer cmd = null;
            if(commandBufferPool.Count == 0)
                cmd = new(service);
            else
                cmd = commandBufferPool.Pop();
            return cmd;
        }

        public void Release(CommandBuffer cmd)
        {
            cmd.Clear();
            commandBufferPool.Push(cmd);
        }

    }
}
