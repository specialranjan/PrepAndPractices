﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Apttus.XAuthor.DynamicsCRMIntegration.SandBox.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IProxyService")]
    public interface IProxyService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProxyService/DoWork", ReplyAction="http://tempuri.org/IProxyService/DoWorkResponse")]
        void DoWork();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProxyService/DoWork", ReplyAction="http://tempuri.org/IProxyService/DoWorkResponse")]
        System.Threading.Tasks.Task DoWorkAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProxyService/CreateAccountwithImporonatedUser", ReplyAction="http://tempuri.org/IProxyService/CreateAccountwithImporonatedUserResponse")]
        System.Guid CreateAccountwithImporonatedUser(string UserId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProxyService/CreateAccountwithImporonatedUser", ReplyAction="http://tempuri.org/IProxyService/CreateAccountwithImporonatedUserResponse")]
        System.Threading.Tasks.Task<System.Guid> CreateAccountwithImporonatedUserAsync(string UserId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProxyService/RetrieveData", ReplyAction="http://tempuri.org/IProxyService/RetrieveDataResponse")]
        void RetrieveData(object service);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProxyService/RetrieveData", ReplyAction="http://tempuri.org/IProxyService/RetrieveDataResponse")]
        System.Threading.Tasks.Task RetrieveDataAsync(object service);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProxyService/getConnectedandReturnAccountName", ReplyAction="http://tempuri.org/IProxyService/getConnectedandReturnAccountNameResponse")]
        string getConnectedandReturnAccountName(string userName, string password, string URL);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProxyService/getConnectedandReturnAccountName", ReplyAction="http://tempuri.org/IProxyService/getConnectedandReturnAccountNameResponse")]
        System.Threading.Tasks.Task<string> getConnectedandReturnAccountNameAsync(string userName, string password, string URL);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IProxyServiceChannel : Apttus.XAuthor.DynamicsCRMIntegration.SandBox.ServiceReference1.IProxyService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProxyServiceClient : System.ServiceModel.ClientBase<Apttus.XAuthor.DynamicsCRMIntegration.SandBox.ServiceReference1.IProxyService>, Apttus.XAuthor.DynamicsCRMIntegration.SandBox.ServiceReference1.IProxyService {
        
        public ProxyServiceClient() {
        }
        
        public ProxyServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ProxyServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProxyServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProxyServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void DoWork() {
            base.Channel.DoWork();
        }
        
        public System.Threading.Tasks.Task DoWorkAsync() {
            return base.Channel.DoWorkAsync();
        }
        
        public System.Guid CreateAccountwithImporonatedUser(string UserId) {
            return base.Channel.CreateAccountwithImporonatedUser(UserId);
        }
        
        public System.Threading.Tasks.Task<System.Guid> CreateAccountwithImporonatedUserAsync(string UserId) {
            return base.Channel.CreateAccountwithImporonatedUserAsync(UserId);
        }
        
        public void RetrieveData(object service) {
            base.Channel.RetrieveData(service);
        }
        
        public System.Threading.Tasks.Task RetrieveDataAsync(object service) {
            return base.Channel.RetrieveDataAsync(service);
        }
        
        public string getConnectedandReturnAccountName(string userName, string password, string URL) {
            return base.Channel.getConnectedandReturnAccountName(userName, password, URL);
        }
        
        public System.Threading.Tasks.Task<string> getConnectedandReturnAccountNameAsync(string userName, string password, string URL) {
            return base.Channel.getConnectedandReturnAccountNameAsync(userName, password, URL);
        }
    }
}