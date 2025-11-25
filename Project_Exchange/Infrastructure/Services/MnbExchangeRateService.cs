using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using BusinessLogic.Managers.Interfaces;
using BusinessLogic.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class MnbExchangeRateService : IMnbExchangeRateService
    {
        private const string Endpoint = "http://www.mnb.hu/arfolyamok.asmx";
        private static readonly XNamespace SoapEnvelope = "http://schemas.xmlsoap.org/soap/envelope/";
        private readonly HttpClient _httpClient;
        private readonly ILogger<MnbExchangeRateService> _logger;

        public MnbExchangeRateService(HttpClient httpClient, ILogger<MnbExchangeRateService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<ExchangeRate>> GetCurrentRatesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var soapEnvelope = BuildSoapEnvelope();
                using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint)
                {
                    Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml")
                };
                request.Headers.Add("SOAPAction", "\"http://www.mnb.hu/webservices/GetCurrentExchangeRates\"");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml")
                {
                    CharSet = Encoding.UTF8.WebName
                };

                using var response = await _httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var payload = await response.Content.ReadAsStringAsync(cancellationToken);
                var innerXml = ExtractResultXml(payload);
                if (string.IsNullOrWhiteSpace(innerXml))
                {
                    _logger.LogWarning("Az MNB SOAP szolgáltatás üres választ adott vissza.");
                    return Array.Empty<ExchangeRate>();
                }

                var document = XDocument.Parse(innerXml);
                var rates = document
                    .Descendants()
                    .Where(node => node.Name.LocalName.Equals("Rate", StringComparison.OrdinalIgnoreCase))
                    .Select(CreateRateFromNode)
                    .Where(rate => rate != null)
                    .Cast<ExchangeRate>()
                    .ToList();

                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Nem sikerült betölteni az árfolyamokat az MNB SOAP szolgáltatásából.");
                return Array.Empty<ExchangeRate>();
            }
        }

        private static string BuildSoapEnvelope() =>
            """
            <?xml version="1.0" encoding="utf-8"?>
            <soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                           xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                           xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <GetCurrentExchangeRates xmlns="http://www.mnb.hu/webservices/" />
              </soap:Body>
            </soap:Envelope>
            """;

        private static string ExtractResultXml(string payload)
        {
            try
            {
                var soapDocument = XDocument.Parse(payload);
                var body = soapDocument.Descendants(SoapEnvelope + "Body").FirstOrDefault();
                var resultNode = body?
                    .Descendants()
                    .FirstOrDefault(node => node.Name.LocalName.Equals("GetCurrentExchangeRatesResult", StringComparison.OrdinalIgnoreCase));

                if (resultNode == null)
                {
                    return string.Empty;
                }

                // the SOAP result is already XML (not encoded). Parse string content
                return XDocument.Parse(resultNode.Value).ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static ExchangeRate? CreateRateFromNode(XElement node)
        {
            var currency = node.Attribute("curr")?.Value;
            var unitAttribute = node.Attribute("unit")?.Value;
            var rawValue = node.Value;

            if (string.IsNullOrWhiteSpace(currency) ||
                string.IsNullOrWhiteSpace(unitAttribute) ||
                string.IsNullOrWhiteSpace(rawValue))
            {
                return null;
            }

            if (!int.TryParse(unitAttribute, NumberStyles.Integer, CultureInfo.InvariantCulture, out var unit))
            {
                unit = 1;
            }

            var normalized = rawValue.Trim().Replace(',', '.');
            if (!decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                return null;
            }

            return new ExchangeRate
            {
                Currency = currency,
                Unit = unit,
                Value = value
            };
        }
    }
}

