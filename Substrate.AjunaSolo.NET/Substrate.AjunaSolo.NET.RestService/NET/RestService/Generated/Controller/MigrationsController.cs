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
    /// MigrationsController controller to access storages.
    /// </summary>
    [ApiController()]
    [Route("[controller]")]
    public sealed class MigrationsController : ControllerBase
    {
        
        private IMigrationsStorage _migrationsStorage;
        
        /// <summary>
        /// MigrationsController constructor.
        /// </summary>
        public MigrationsController(IMigrationsStorage migrationsStorage)
        {
            _migrationsStorage = migrationsStorage;
        }
        
        /// <summary>
        /// >> Cursor
        ///  The currently active migration to run and its cursor.
        /// 
        ///  `None` indicates that no migration is running.
        /// </summary>
        [HttpGet("Cursor")]
        [ProducesResponseType(typeof(Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_migrations.EnumMigrationCursor), 200)]
        [StorageKeyBuilder(typeof(Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.MigrationsStorage), "CursorParams")]
        public IActionResult GetCursor()
        {
            return this.Ok(_migrationsStorage.GetCursor());
        }
        
        /// <summary>
        /// >> Historic
        ///  Set of all successfully executed migrations.
        /// 
        ///  This is used as blacklist, to not re-execute migrations that have not been removed from the
        ///  codebase yet. Governance can regularly clear this out via `clear_historic`.
        /// </summary>
        [HttpGet("Historic")]
        [ProducesResponseType(typeof(Substrate.NetApi.Model.Types.Base.BaseTuple), 200)]
        [StorageKeyBuilder(typeof(Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.MigrationsStorage), "HistoricParams", typeof(Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT14))]
        public IActionResult GetHistoric(string key)
        {
            return this.Ok(_migrationsStorage.GetHistoric(key));
        }
    }
}
