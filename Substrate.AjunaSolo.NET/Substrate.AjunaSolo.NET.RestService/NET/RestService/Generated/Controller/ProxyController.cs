//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Substrate.AjunaSolo.NET.RestService.Generated.Storage;
using Substrate.NetApi.Model.Types.Base;
using Substrate.ServiceLayer.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Substrate.AjunaSolo.NET.RestService.Generated.Controller
{
    
    
    /// <summary>
    /// ProxyController controller to access storages.
    /// </summary>
    [ApiController()]
    [Route("[controller]")]
    public sealed class ProxyController : ControllerBase
    {
        
        private IProxyStorage _proxyStorage;
        
        /// <summary>
        /// ProxyController constructor.
        /// </summary>
        public ProxyController(IProxyStorage proxyStorage)
        {
            _proxyStorage = proxyStorage;
        }
        
        /// <summary>
        /// >> Proxies
        ///  The set of account proxies. Maps the account which has delegated to the accounts
        ///  which are being delegated to, together with the amount held on deposit.
        /// </summary>
        [HttpGet("Proxies")]
        [ProducesResponseType(typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT26, Substrate.NetApi.Model.Types.Primitive.U128>), 200)]
        [StorageKeyBuilder(typeof(Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.ProxyStorage), "ProxiesParams", typeof(Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32))]
        public IActionResult GetProxies(string key)
        {
            return this.Ok(_proxyStorage.GetProxies(key));
        }
        
        /// <summary>
        /// >> Announcements
        ///  The announcements made by the proxy (key).
        /// </summary>
        [HttpGet("Announcements")]
        [ProducesResponseType(typeof(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT27, Substrate.NetApi.Model.Types.Primitive.U128>), 200)]
        [StorageKeyBuilder(typeof(Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.ProxyStorage), "AnnouncementsParams", typeof(Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32))]
        public IActionResult GetAnnouncements(string key)
        {
            return this.Ok(_proxyStorage.GetAnnouncements(key));
        }
    }
}
