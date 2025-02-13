using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.enums;
using Substrate.NetApi.Model.Types.Base;
using System;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    public class CasinoActionSharp
    {
        public CasinoActionSharp(EnumCasinoAction casinoAction)
        {
            CasinoAction = casinoAction.Value;
            switch(casinoAction.Value)
            {
                case CasinoAction.Create:
                    {
                        CasinoAction = CasinoAction.Create;
                        AssetType = new AssetTypeSharp((EnumAssetType)casinoAction.Value2);
                        break;
                    }
                case CasinoAction.Deposit:
                    {
                        CasinoAction = CasinoAction.Deposit;
                        var tuple = (BaseTuple<EnumAssetType, EnumTokenType>)casinoAction.Value2;
                        AssetType = new AssetTypeSharp((EnumAssetType)tuple.Value[0]);
                        TokenType = ((EnumTokenType)tuple.Value[1]).Value;
                        break;
                    }
                case CasinoAction.Gamble:
                    {
                        CasinoAction = CasinoAction.Gamble;
                        MultiplierType = ((EnumMultiplierType)casinoAction.Value2).Value;
                        break;
                    }
                case CasinoAction.Withdraw:
                    {
                        CasinoAction = CasinoAction.Withdraw;
                        var tuple = (BaseTuple<EnumAssetType, EnumTokenType>)casinoAction.Value2;
                        AssetType = new AssetTypeSharp((EnumAssetType)tuple.Value[0]);
                        TokenType = ((EnumTokenType)tuple.Value[1]).Value;
                        break;
                    }
                case CasinoAction.Rent:
                    {
                        CasinoAction = CasinoAction.Rent;
                        MultiplierType = ((EnumMultiplierType)casinoAction.Value2).Value;
                        break;
                    }
                case CasinoAction.Reserve:
                    {
                        CasinoAction = CasinoAction.Reserve;
                        MultiplierType = ((EnumMultiplierType)casinoAction.Value2).Value;
                        break;
                    }
                case CasinoAction.Release:
                    {
                        CasinoAction = CasinoAction.Release;
                        break;
                    }
                case CasinoAction.Kick:
                    {
                        CasinoAction = CasinoAction.Kick;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        // Parameterless constructor for deserialization
        public CasinoActionSharp() { }

        public EnumCasinoAction ToSubstrate()
        {
            var result = new EnumCasinoAction();
            result.Value = CasinoAction;
            switch(CasinoAction)
            {
                case CasinoAction.Create:
                    {
                        if (AssetType == null)
                        {
                            throw new Exception($"AssetType is required for {CasinoAction}");
                        }
                        result.Value2 = AssetType.ToSubstrate();
                        break;
                    }
                case CasinoAction.Deposit:
                    {
                        if (AssetType == null)
                        {
                            throw new Exception($"AssetType is required for {CasinoAction}");
                        }
                        if (TokenType == null)
                        {
                            throw new Exception($"TokenType is required for {CasinoAction}");
                        }
                        var tokenType = new EnumTokenType();
                        tokenType.Create(TokenType.Value);
                        result.Value2 = new BaseTuple<EnumAssetType, EnumTokenType>(AssetType.ToSubstrate(), tokenType);
                        break;
                    }
                case CasinoAction.Gamble:
                    {
                        if (MultiplierType == null)
                        {
                            throw new Exception($"MultiplierType is required for {CasinoAction}");
                        }
                        var multiplierType = new EnumMultiplierType();
                        multiplierType.Create(MultiplierType.Value);
                        result.Value2 = new BaseEnum<MultiplierType>(multiplierType);
                        break;
                    }
                case CasinoAction.Withdraw:
                    {
                        if (AssetType == null)
                        {
                            throw new Exception($"AssetType is required for {CasinoAction}");
                        }
                        if (TokenType == null)
                        {
                            throw new Exception($"TokenType is required for {CasinoAction}");
                        }
                        var tokenType = new EnumTokenType();
                        tokenType.Create(TokenType.Value);
                        result.Value2 = new BaseTuple<EnumAssetType, EnumTokenType>(AssetType.ToSubstrate(), tokenType);
                        break;
                    }
                case CasinoAction.Rent:
                    {
                        if (MultiplierType == null)
                        {
                            throw new Exception($"MultiplierType is required for {CasinoAction}");
                        }
                        var multiplierType = new EnumMultiplierType();
                        multiplierType.Create(MultiplierType.Value);
                        result.Value2 = new BaseEnum<MultiplierType>(multiplierType);
                        break;
                    }
                case CasinoAction.Reserve:
                    {
                        if (MultiplierType == null)
                        {
                            throw new Exception($"MultiplierType is required for {CasinoAction}");
                        }
                        var multiplierType = new EnumMultiplierType();
                        multiplierType.Create(MultiplierType.Value);
                        result.Value2 = new BaseEnum<MultiplierType>(multiplierType);
                        break;
                    }
                case CasinoAction.Release:
                    {
                        result.Value2 = new BaseVoid();
                        break;
                    }
                case CasinoAction.Kick:
                    {
                        result.Value2 = new BaseVoid();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            };
            return result;
        }

        /// <summary>
        /// CasinoAction
        /// </summary>
        public CasinoAction CasinoAction { get; set; }

        /// <summary>
        /// AssetType
        /// </summary>
        public AssetTypeSharp? AssetType { get; set; }

        /// <summary>
        /// MultiplierType
        /// </summary>
        public MultiplierType? MultiplierType { get; set; }

        /// <summary>
        /// TokenType
        /// </summary>
        public TokenType? TokenType { get; set; }

    }
}