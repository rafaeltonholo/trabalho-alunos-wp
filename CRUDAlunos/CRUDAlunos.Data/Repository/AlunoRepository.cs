using CRUDAlunos.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using CRUDAlunos.Domain.Entities;
using AutoMapper;
using CRUDAlunos.Data.DataObjects;
using System.Linq;

namespace CRUDAlunos.Data.Repository {
    public class AlunoRepository : BaseRepository<AlunoData>, IAlunoRepository {
        public void Add(Aluno entidade) {
            var map = Mapper.Map<Aluno, AlunoData>(entidade);
            base.Add(map);
        }

        public new IEnumerable<Aluno> FindAll() {
            var all = base.FindAll();
            return Mapper.Map<List<AlunoData>, List<Aluno>>(all.ToList());
        }

        public new Aluno FindById(int id) {
            var item = base.FindById(id);
            return Mapper.Map<AlunoData, Aluno>(item);
        }

        public void Remove(Aluno entidade) {
            var map = Mapper.Map<Aluno, AlunoData>(entidade);
            base.Remove(map);
        }

        public void Update(Aluno entidade) {
            var map = Mapper.Map<Aluno, AlunoData>(entidade);
            base.Update(map);
        }
    }
}
