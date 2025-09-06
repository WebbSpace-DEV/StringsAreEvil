using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime;
using System.Text;
using StringsAreEvil.API.DTOs;
using Global.Utility.Options;
using Microsoft.Extensions.Options;

namespace StringsAreEvil.API.Controllers.Benchmark;

//[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/Benchmark/[controller]")]
public class StringsAreEvilController : ControllerBase
{
    private long _max;

    public StringsAreEvilController(IOptions<AppSettingsOption> options)
    {
        long.TryParse(options.Value.MaxIterations!, out _max);
    }

    [HttpGet("GetProof/{isManaged}/{isString}/{iterations}/", Name = "GetProof")]
    public IActionResult GetProof(bool isManaged, bool isString, long iterations)
    {
        string output;
        StringBuilder message = new StringBuilder();
        GCLatencyMode initMode = GCSettings.LatencyMode;

        Stopwatch timer = new Stopwatch();

        #region GC_alpha
        GC.Collect();
        if (!isManaged)
        {
            /*
             * 
             * Kinda turn Garbage Collection off.
             * 
             */
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            GC.TryStartNoGCRegion(0xffffffff, true);
        }
        #endregion

        /*
         * 
         * Get the pre-processing memory consumed.
         * 
         */
        var alpha = GC.GetTotalMemory(false);
        var omega = alpha;
        /*
         * 
         * Start the timer.
         * 
         */
        timer.Start();

        long max = iterations < _max ? iterations : _max;

        if (isString)
        {
            string s = String.Empty;

            for (long i = 0; i < max; i++)
            {
                s += ".";
            }

            output = s;
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < max; i++)
            {
                sb.Append(".");
            }

            output = sb.ToString();
        }
        /*
         * 
         * Stop the timer.
         * 
         */
        timer.Stop();
        /*
         * 
         * Get the post-processing memory consumed.
         * 
         */
        omega = GC.GetTotalMemory(false);

        #region GC_omega
        if (!isManaged)
        {
            /*
             * 
             * Restore Garbage Collection.
             * 
             */
            GC.EndNoGCRegion();
            GCSettings.LatencyMode = initMode;
        }
        GC.Collect();
        #endregion

        message.AppendFormat("GC: {0} / Type: {1} / Iterations: {2:n0} / Execution Time: {3:n4} ms / Virtual Memory Consumed: {4:n0} bytes",
            isManaged ? "Yes" : "No",
            isString ? "String" : "StringBuilder",
            max,
            timer.Elapsed.TotalMilliseconds,
            omega - alpha);

        Debug.WriteLine(String.Format("[{0}]", output));

        return Ok(new MessageDto { Value = message.ToString() });
    }
}
