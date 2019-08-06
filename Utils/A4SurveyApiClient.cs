using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Net;
using ASJ.Models;

namespace ASJ.Utils
{
    public class A4SurveyApiClient
    {
        #region Attributes

        private string apiUrl = "";
        public string ApiUrl { get { return apiUrl; } protected set { apiUrl = value; } }

        private string apiAccessKey = "";
        public string ApiAccessKey { get { return apiAccessKey; } protected set { apiAccessKey = value; } }

        #endregion

        #region Initialization

        public A4SurveyApiClient(string apiUrl, string apiAccessKey)
        {
            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                throw new Exception("Invalid apiUrl");
            }
            if (string.IsNullOrWhiteSpace(apiAccessKey))
            {
                throw new Exception("Invalid apiAccessKey");
            }

            ApiUrl = apiUrl;
            ApiAccessKey = apiAccessKey;
        }

        #endregion

        public IEnumerable<Survey> GetSurveys()
        {
            try
            {
                string resultString = Get(ApiUrl + "/users/user/surveys");
                UserSurveys surveys = JsonHelper.Deserialize<UserSurveys>(resultString);
                return surveys.Surveys;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                throw;
            }
        }

        public List<QuestionVox> GetQuestions(int surveyId)
        {
            try
            {
                string resultString = Get(ApiUrl + "/survey/question/" + surveyId.ToString());
                var questions = JsonHelper.Deserialize<List<QuestionVox>>(resultString);
                return questions;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                throw;
            }
        }

        public IEnumerable<ExtractionResult> GetExtractions(int surveyId)
        {
            try
            {
                string result = Get(ApiUrl + "/results/extract?surveyId=" + surveyId);

                SurveyExtractionsResult extractionsResult = JsonHelper.Deserialize<SurveyExtractionsResult>(result);

                return extractionsResult.Extractions;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieve the execution status of an extraction task
        /// </summary>
        /// <param name="extractionId"></param>
        /// <returns>Returns the meta data of the extraction task: Extraction Id, File Id, Status</returns>
        public ExtractionResult GetExtractionStatus(int extractionId)
        {
            try
            {
                string result = Get(ApiUrl + "/results/extract/" + extractionId.ToString());

                ExtractionResult extractionResult = JsonHelper.Deserialize<ExtractionResult>(result);

                return extractionResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieve the ProjectData of an extraction task.
        /// </summary>
        /// <param name="extractionId"></param>
        /// <remarks>
        /// Raises and execption if the task is not completed
        /// </remarks>
        /// <returns></returns>
        public byte[] GetExtractionData(int extractionId)
        {
            try
            {
                ExtractionResult extractionResult = GetExtractionStatus(extractionId);

                if (extractionResult != null && extractionResult.FileId > 0)
                {
                    MemoryStream stream = GetStream(ApiUrl + "/results/extract/" + string.Format("?extractionId={0}&fileId={1}", extractionResult.ExtractionId, extractionResult.FileId));
                    return stream.ToArray();
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                throw;
            }
        }

        public ExtractionResult CreateExtraction(int surveyId, string name, string[] variables = null, string language = null, DateTime? startDate = null)
        {
            if (surveyId <= 0)
            {
                throw new Exception(string.Format("Invalid Survey Id {0}", surveyId));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = string.Format("Extraction_SAV_{0}", Guid.NewGuid());
            }

            Extraction extraction = new Extraction
            {
                Name = name,
                IncludeLabels = true,
                IncludeOpenEnds = true,
                SurveyId = surveyId,
                Language = language ?? "en",
                StripHtmlFromLabels = true,
                Variables = variables,
                IncludeHeader = true,
                ExtractFormat = "SAV",
                UseChoiceLabels = true,
                IncludeConnectionHistory = true,
                DapresyDataFormat = true
            };

            if (startDate.HasValue)
            {
                extraction.Filter = new CaseFilter { LastActivity = new LastActivityFilter { Begin = startDate, End = DateTime.UtcNow } };
            }

            return CreateExtraction(extraction);
        }

        /// <summary>
        /// Create and execute a new extraction task that will be executed asynchronously by A4Survey.
        /// </summary>
        /// <param name="extraction"></param>
        /// <returns>Returns the meta data of the extraction task: Extraction Id, File Id, Status</returns>
        public ExtractionResult CreateExtraction(Extraction extraction)
        {
            try
            {
                string resultString = Post("/results/extract", extraction);
                var extractionResult = JsonHelper.Deserialize<ExtractionResult>(resultString);
                return extractionResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                throw;
            }
        }

        public void StartExtraction(int extractionId)
        {
            Put(string.Format("{0}/results/extract/{1}/start", ApiUrl, extractionId));
        }

        public void StopExtraction(int extractionId)
        {
            Put(string.Format("{0}/results/extract/{1}/stop", ApiUrl, extractionId));
        }

        public ImportSampleResult CreateSampleImportation(int surveyId, string language, MappingDefinition[] variableMappings, byte[] data)
        {
            if (surveyId <= 0)
            {
                throw new Exception(string.Format("Invalid Survey Id {0}", surveyId));
            }

            SampleDefinition sample = new SampleDefinition
            {
                Name = string.Format("ImportSample_{0}", Guid.NewGuid()),
                SurveyId = surveyId,
                Language = language ?? "en",
                VariableMappings = variableMappings,
                FileData = data,
                ImportFormat = "CSV",
                ImportationType = "Insert",
                HeaderRowIncluded = true

            };

            return CreateSampleImportation(sample);
        }

        public ImportSampleResult CreateSampleImportation(SampleDefinition sample)
        {
            try
            {
                string resultString = Post("/sample/import", sample);
                var extractionResult = JsonHelper.Deserialize<ImportSampleResult>(resultString);
                return extractionResult;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                throw;
            }
        }


        #region Helper methods

        private string Post(string method, object data)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ApiUrl + method);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Headers.Add("Authorization", "Client " + ApiAccessKey);

            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(JsonHelper.Serialize(data));
            streamOut.Close();

            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();
            return strResponse;
        }

        private string Get(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.Method = "GET";
            req.Headers.Add("Authorization", "Client " + ApiAccessKey);

            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();
            return strResponse;
        }

        private HttpStatusCode Put(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "PUT";
            request.ContentLength = 0;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", "Client " + ApiAccessKey);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response.StatusCode;
        }

        private MemoryStream GetStream(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            //Set values for the request back
            req.Method = "GET";
            //req.ContentType = "json";
            req.Headers.Add("Authorization", "Client " + ApiAccessKey);

            Stream responseStream = req.GetResponse().GetResponseStream();
            MemoryStream stream = new MemoryStream();
            responseStream.CopyTo(stream);
            return stream;
        }

        #endregion
    }
}
