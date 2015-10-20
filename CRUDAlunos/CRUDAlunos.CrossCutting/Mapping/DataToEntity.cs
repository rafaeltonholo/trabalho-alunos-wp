using AutoMapper;
using CRUDAlunos.Data.DataObjects;
using CRUDAlunos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAlunos.CrossCutting.Mapping {
    public class DataToEntity : Profile {
        public override string ProfileName {
            get {
                return "DataToEntityProfile";
            }
        }

        protected override void Configure() {
            base.Configure();
            Mapper.CreateMap<AlunoData, Aluno>();
        }
    }
}