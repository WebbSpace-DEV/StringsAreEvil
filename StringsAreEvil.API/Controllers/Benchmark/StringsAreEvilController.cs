using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime;
using System.Text;
using StringsAreEvil.API.DTOs;

namespace StringsAreEvil.API.Controllers.Benchmark;

//[Authorize]
[AllowAnonymous]
[ApiController]
[Route("api/Benchmark/[controller]")]
public class StringsAreEvilController : ControllerBase
{
    private const string _string = ".";
    private const char _char = '.';

    public StringsAreEvilController()
    {
    }

    [HttpGet("GetProof/{isManaged}/{isString}/{iterations}/", Name = "StringsAreEvilGetProof")]
    public async Task<IActionResult> StringsAreEvilGetProof(bool isManaged, bool isString, long iterations)
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

        if (isString)
        {
            string s = String.Empty;

            for (int i = 0; i < iterations; i++)
            {
                s += ".";
                //s += _string;
                //s += '.';
                //s += _char;
            }

            output = s;
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < iterations; i++)
            {
                sb.Append(".");
                //sb.Append(_string);
                //sb.Append('.');
                //sb.Append(_char);
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
            iterations,
            timer.Elapsed.TotalMilliseconds,
            omega - alpha);

        Debug.WriteLine(String.Format("[{0}]", output));

        return Ok(new MessageDto { Value = message.ToString() });
    }
}
