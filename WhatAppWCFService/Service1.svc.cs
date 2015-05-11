using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WhatsAppApi;
using WhatsAppPort;
using WhatAppWCFService;

namespace WhatAppWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        string IService1.RequestCode(string number, out string password, string method)
        {
            //method = "sms";
            if (WhatsAppApi.Register.WhatsRegisterV2.RequestCode(number, out password, method))
            return password;
            else
                return null;
        }
        string IService1.RegisterCode(string number, string code)
        {
            
            string pwd = WhatsAppApi.Register.WhatsRegisterV2.RegisterCode(number, code);

            return pwd;
        }
        string IService1.CheckLogin(string number, string password)
        {

            if (WhatsAppPort.frmLogin.CheckLogin(number, password))
                return "success";
            else
                return "fail";


        }
    }
}
