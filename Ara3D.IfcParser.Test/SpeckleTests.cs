﻿using Ara3D.IfcLoader;
using Ara3D.IfcParser.Test;
using Ara3D.Speckle.Data;
using Ara3D.Utils;
using Ara3D.Logging;
using Ara3D.Speckle.IfcLoader;
using NUnit.Framework;
using static Ara3D.IfcParser.Test.Config;
using Base = Speckle.Core.Models.Base;
using Speckle.Core.Credentials;
using Speckle.Core.Api;
using Speckle.Automate.Sdk.Schema;
using Speckle.Automate.Sdk;

namespace WebIfcDotNetTests
{
    public static class SpeckleTests
    {
        public static Base IfcFileToBase(FilePath fp, ILogger logger)
        {
            var f = IfcFile.Load(fp, true, logger);
            logger?.Log("Converting to speckle");
            var r = f.ToSpeckle();
            logger?.Log("Conversion completed");
            return r;
        }

        public static IEnumerable<FilePath> InputFiles
            => Files;

        [TestCaseSource(nameof(InputFiles))]
        public static void IfcFileToSpeckleToJson(FilePath f)
        {
            var logger = CreateLogger();
            IfcFileToSpeckle(f, logger, false);
        }

        public static Base IfcFileToSpeckle(FilePath f, ILogger logger, bool showJson = false)
        {
            var b = IfcFileToBase(f, logger);

            var convertedRoot = b.ToSpeckleObject();

            var tmp = PathUtil.CreateTempFile("json");
            var json = convertedRoot.ToJson();
            tmp.WriteAllText(json);
            logger?.Log($"Wrote json to: {tmp}");
                
            if (showJson)   
                tmp.OpenFile();

            return b;
        }

        [Test, Explicit]
        public static void ListModelsInProject()
        {
            SpeckleUtils.ListModels(CreateLogger(), Config.TestProjectId);
        }

        [Test, Explicit]
        public static void PushAc20Haus()
        {
            var logger = CreateLogger();
            var f = AC20Haus;
            logger?.Log($"Converting {f} to Speckle");
            var b = IfcFileToBase(f, logger);
            logger?.Log($"Conversion completed");
            var client = SpeckleUtils.LoginDefaultClient(logger);
            var result = client.PushModel(TestProjectId, f.GetFileName(), b, logger);
            logger?.Log(result);
            logger?.Log($"Pushed to Speckle at {TestProjectUrl}");
        }

        [Test, Explicit]
        public static void PushAllTestFiles()
        {
            var inputFiles = Files;
            foreach (var f in inputFiles)
            {
                var logger = CreateLogger();
                try
                {
                    logger?.Log($"Converting {f} to Speckle");
                    var b = IfcFileToBase(f, logger);
                    logger?.Log($"Conversion completed");
                    var client = SpeckleUtils.LoginDefaultClient(logger);
                    var result = client.PushModel("d1553c3803", f.GetFileName(), b, logger);
                    logger?.Log(result);
                }
                catch (Exception e)
                {
                    logger?.LogError(e);
                }
            }
        }

        [Test, Explicit]
        public static void TestPushFile()
        {
            /*
            var logger = CreateLogger();
            SpeckleUtils.LoginDefaultClient(logger);
            var project = "6b218f1aca";
            var baseUrl = "https://app.speckle.systems/";
            var result = SpeckleUtils.PushToServer(baseUrl, project, AC20Haus, logger);
            logger.Log(result);
            */
        }

        [Test,Explicit]
        public static void PushUsingAutomate()
        {
            var logger = CreateLogger();
            var client = SpeckleUtils.LoginDefaultClient(logger);
            logger.Log($"Logged into account {client.Account} with user {client.ActiveUser}");
            
            // Initialize the automation context
            var remoteRunData = new AutomationRunData
            {
                SpeckleServerUrl = "https://app.speckle.systems/",
                ProjectId = "6b218f1aca",
            };

            
            var localRunData = new AutomationRunData
            {
                SpeckleServerUrl = "http://127.0.0.1/",
                ProjectId = "e9f309708a",
            };

            /*
            var context = AutomationContext.Initialize(localRunData, client.ApiToken).Result;
            logger.Log($"Created automation context with status {context.RunStatus}");
            var task = context.StoreFileResult(AC20Haus);
            Task.WaitAll(task);
            logger.Log($"Stored file with status {context.RunStatus}");
            */

            var projectId = remoteRunData.ProjectId;
            var url = remoteRunData.SpeckleServerUrl;
            var modelId = "20ad8406cc";
            SpeckleUtils.UploadFile(client, url, projectId, modelId, AC20Haus, logger);
        }


    }
}
