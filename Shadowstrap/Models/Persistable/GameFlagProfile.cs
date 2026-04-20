namespace Shadowstrap.Models.Persistable
{
    public class GameFlagProfile
    {
        public Guid   Id      { get; set; } = Guid.NewGuid();
        public string Name    { get; set; } = string.Empty;
        public long   PlaceId { get; set; }
        public bool   Enabled { get; set; } = true;

        /// <summary>Flag key → value string pairs to write on game join.</summary>
        public Dictionary<string, string> Flags { get; set; } = new();

        // ── built-in presets ─────────────────────────────────────────────────
        public static readonly IReadOnlyList<GameFlagProfile> BuiltIn = new GameFlagProfile[]
        {
            new()
            {
                Name    = "The Strongest Battlegrounds",
                PlaceId = 2476220120,
                Flags   = new()
                {
                    ["DFIntTaskSchedulerTargetFps"]               = "9999",
                    ["FIntRenderShadowIntensity"]                 = "0",
                    ["FFlagGlobalWindRendering"]                  = "False",
                    ["FFlagGlobalWindActivated"]                  = "False",
                    ["DFIntCSGLevelOfDetailSwitchingDistance"]    = "0",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL12"] = "0",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL23"] = "0",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL34"] = "0",
                }
            },
            new()
            {
                Name    = "Blox Fruits",
                PlaceId = 2753915549,
                Flags   = new()
                {
                    ["DFIntTaskSchedulerTargetFps"] = "9999",
                    ["FIntRenderShadowIntensity"]   = "1",
                    ["FFlagGlobalWindRendering"]    = "False",
                    ["FFlagGlobalWindActivated"]    = "False",
                }
            },
            new()
            {
                Name    = "Murder Mystery 2",
                PlaceId = 142823291,
                Flags   = new()
                {
                    ["DFIntTaskSchedulerTargetFps"] = "9999",
                    ["FIntRenderShadowIntensity"]   = "0",
                }
            },
            new()
            {
                Name    = "Jailbreak",
                PlaceId = 606849621,
                Flags   = new()
                {
                    ["DFIntTaskSchedulerTargetFps"]               = "9999",
                    ["FIntRenderShadowIntensity"]                 = "0",
                    ["FFlagGlobalWindRendering"]                  = "False",
                    ["FFlagGlobalWindActivated"]                  = "False",
                    ["DFIntCSGLevelOfDetailSwitchingDistance"]    = "0",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL12"] = "0",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL23"] = "0",
                    ["DFIntCSGLevelOfDetailSwitchingDistanceL34"] = "0",
                }
            },
            new()
            {
                Name    = "Arsenal",
                PlaceId = 286090429,
                Flags   = new()
                {
                    ["DFIntTaskSchedulerTargetFps"] = "9999",
                    ["FIntRenderShadowIntensity"]   = "0",
                    ["FFlagGlobalWindRendering"]    = "False",
                    ["FFlagGlobalWindActivated"]    = "False",
                }
            },
            new()
            {
                Name    = "Da Hood",
                PlaceId = 2788229376,
                Flags   = new()
                {
                    ["DFIntTaskSchedulerTargetFps"] = "9999",
                    ["FIntRenderShadowIntensity"]   = "0",
                    ["FFlagGlobalWindRendering"]    = "False",
                    ["FFlagGlobalWindActivated"]    = "False",
                }
            },
            new()
            {
                Name    = "Brookhaven RP",
                PlaceId = 4924922222,
                Flags   = new()
                {
                    ["DFIntTaskSchedulerTargetFps"] = "9999",
                    ["FFlagGlobalWindRendering"]    = "False",
                    ["FFlagGlobalWindActivated"]    = "False",
                }
            },
            new()
            {
                Name    = "Pet Simulator 99",
                PlaceId = 8737899170,
                Flags   = new()
                {
                    ["DFIntTaskSchedulerTargetFps"] = "9999",
                    ["FIntRenderShadowIntensity"]   = "1",
                    ["FFlagGlobalWindRendering"]    = "False",
                    ["FFlagGlobalWindActivated"]    = "False",
                }
            },
        };
    }
}
