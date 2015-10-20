using AutoMapper;
using CRUDAlunos.Aplicacao.ViewObjects;
using CRUDAlunos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAlunos.CrossCutting.Mapping {
    public class ViewModelToEntity : Profile {
        public override string ProfileName {
            get {
                return "ViewModelToEntityProfile";
            }
        }

        protected override void Configure() {
            base.Configure();
            Mapper.CreateMap<AlunoView, Aluno>();
        }
    }
}
