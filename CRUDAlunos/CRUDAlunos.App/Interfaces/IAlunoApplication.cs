using CRUDAlunos.Aplicacao.ViewObjects;
using CRUDAlunos.Aplicacao.Interfaces.Base;
using CRUDAlunos.Domain.Entities;

namespace CRUDAlunos.Aplicacao.Interfaces {
    public interface IAlunoApplication : IBaseApplication<AlunoView, Aluno> {
    }
}
