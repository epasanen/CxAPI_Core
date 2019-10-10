using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxAPI_Core
{
    class dispatcher
    {
        public resultClass dispatch()
        {
            resultClass token = Configuration.mono_command_args();
            if (token.status != 0) { return token; }
            secure secure = new secure(token);
            _options.debug = token.debug;
            _options.level = token.verbosity;

            switch (token.api_action)
            {
                case api_action.getToken:
                    {
                        fetchToken newtoken = new fetchToken();
                        newtoken.get_token(secure.decrypt_Credentials());
                        break;
                    }
                case api_action.storeCredentials:
                    {
                        storeCredentials cred = new storeCredentials();
                        token = cred.save_credentials(token);
                        break;
                    }
                case api_action.scanResults:
                    {
                        if (token.report_name.Contains("REST"))
                        {
                            using (CxRest_API cxRest_API = new CxRest_API(token))
                            {
                                if (token.report_name == "REST_REPORT_1")
                                {
                                    cxRest_API.fetchReportsbyDate();
                                }
                            }
                        }
                        else { }
                        using (CxSoapSDK cxSoapSDK = new CxSoapSDK(token))
                        {
                            if (token.report_name == "REPORT_1")
                            {
                                cxSoapSDK.makeProjectScanCsv_1();
                            }
                            if (token.report_name == "REPORT_2")
                            {
                                cxSoapSDK.makeProjectScanCsv_2();
                            }
                        }
          
                        break;
                    }
                case api_action.getProjects:
                    {
                        getProjects getProjects = new getProjects();
                        getProjects.get_projects(token);
                        break;
                    }

            }
            return token;
        }
    }
}
