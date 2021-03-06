﻿using System;
using System.Threading.Tasks;
using MassTransitDemo.Contract;

using static System.Console;

namespace MassTransitDemo.Client
{
    public static class Extensions
    {
        public static async Task WriteResultAsync(this Task<ICommandValidationResult> resultTask)
        {
            var result = await resultTask;
            if (result != null)
            {
                var status = result.Fail ? "Fail" : "Success";
                await Out.WriteLineAsync($"Result: {status}, {result.Message}");
            }
        }
    }
}