using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RadialBlurFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class RadialBlurSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Material blurMaterial = null;
    }

    public RadialBlurSettings settings = new RadialBlurSettings();
    private RadialBlurPass radialBlurPass;

    public override void Create()
    {
        radialBlurPass = new RadialBlurPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.blurMaterial == null) return;
        
        radialBlurPass.ConfigureInput(ScriptableRenderPassInput.Color);
        renderer.EnqueuePass(radialBlurPass);
    }

    class RadialBlurPass : ScriptableRenderPass
    {
        private RadialBlurSettings settings;
        private RTHandle tempHandle;
        private string profilerTag;

        public RadialBlurPass(RadialBlurSettings settings)
        {
            this.settings = settings;
            renderPassEvent = settings.renderPassEvent;
            profilerTag = "RadialBlur";
        }

        [System.Obsolete]
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            
            RenderingUtils.ReAllocateHandleIfNeeded(ref tempHandle, descriptor, name: "_TempRadialBlurTexture");
        }

        [System.Obsolete]
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (settings.blurMaterial == null) return;

            var cmd = CommandBufferPool.Get(profilerTag);
            
            var cameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
            
            // Copy source to temp texture
            cmd.Blit(cameraColorTarget, tempHandle);
            
            // Apply radial blur effect
            cmd.Blit(tempHandle, cameraColorTarget, settings.blurMaterial);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // Cleanup is handled automatically by RTHandle system
        }

        public void Dispose()
        {
            tempHandle?.Release();
        }
    }

    protected override void Dispose(bool disposing)
    {
        radialBlurPass?.Dispose();
    }
}
