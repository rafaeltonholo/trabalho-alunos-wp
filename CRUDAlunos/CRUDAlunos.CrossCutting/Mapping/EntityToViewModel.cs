using AutoMapper;
using CRUDAlunos.Aplicacao.ViewObjects;
using CRUDAlunos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAlunos.CrossCutting.Mapping {
    public class EntityToViewModel : Profile {
        public override string ProfileName {
            get {
                return "EntityToViewModelProfile";
            }
        }
        protected override void Configure() {
            base.Configure();
            Mapper.CreateMap<Aluno, AlunoView>();
        }
    }
}
