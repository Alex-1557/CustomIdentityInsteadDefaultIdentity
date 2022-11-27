Imports System.Threading
Imports BackendAPI.FrontEndModels
Imports FrontEndData.Models
Imports Microsoft.AspNetCore.Identity

Namespace Services
    Public Interface ICustomRoleStore
        Inherits IRoleStore(Of ApplcationRole)
        Overloads Sub Dispose()
        Overloads Function CreateAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of IdentityResult)
        Overloads Function DeleteAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of IdentityResult)
        Overloads Function FindByIdAsync(roleId As String, cancellationToken As CancellationToken) As Task(Of ApplcationRole)
        Overloads Function FindByNameAsync(normalizedRoleName As String, cancellationToken As CancellationToken) As Task(Of ApplcationRole)
        Overloads Function GetNormalizedRoleNameAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of String)
        Overloads Function GetRoleIdAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of String)
        Overloads Function GetRoleNameAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of String)
        Overloads Function SetNormalizedRoleNameAsync(role As ApplcationRole, normalizedName As String, cancellationToken As CancellationToken) As Task
        Overloads Function SetRoleNameAsync(role As ApplcationRole, roleName As String, cancellationToken As CancellationToken) As Task
        Overloads Function UpdateAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of IdentityResult)
    End Interface
End Namespace