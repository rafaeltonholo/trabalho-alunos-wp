using CRUDAlunos.Aplicacao.ViewObjects;
using CRUDAlunos.Domain.Entities;

namespace CRUDAlunos.Aplicacao.Interfaces {
    public class AlunoApplication : BaseApplication<AlunoView, Aluno>, IAlunoApplication {
    }
}
