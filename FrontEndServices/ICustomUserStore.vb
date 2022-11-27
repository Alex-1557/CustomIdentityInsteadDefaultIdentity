Imports System.Threading
Imports BackendAPI.Model
Imports FrontEndData.Models
Imports Microsoft.AspNetCore.Identity
Namespace Services
    Public Interface ICustomUserStore(Of T As Class)
        Inherits IUserStore(Of T)
        Inherits IUserPasswordStore(Of T)
        Inherits IUserRoleStore(Of T)
        Inherits IUserEmailStore(Of T)
        Inherits IUserPhoneNumberStore(Of T)
        Inherits IUserAuthenticatorKeyStore(Of T) 'TwoFactor
        Inherits IUserTwoFactorStore(Of T)        'TwoFactor
        Inherits IUserTwoFactorRecoveryCodeStore(Of T)        'TwoFactor
        Inherits IUserLoginStore(Of T) 'download personal data
        Inherits IUserLockoutStore(Of T)
        Overloads Function SetPasswordHashAsync(user As T, passwordHash As String, cancellationToken As CancellationToken) As Task
        Overloads Function GetPasswordHashAsync(user As T, cancellationToken As CancellationToken) As Task(Of String)
        Overloads Function HasPasswordAsync(user As T, cancellationToken As CancellationToken) As Task(Of Boolean)
        'IUserRoleStore
        Overloads Function AddToRoleAsync(user As T, roleName As String, cancellationToken As CancellationToken) As Task
        Overloads Function RemoveFromRoleAsync(user As T, roleName As String, cancellationToken As CancellationToken) As Task
        Overloads Function GetRolesAsync(user As T, cancellationToken As CancellationToken) As Task(Of IList(Of String))
        Overloads Function IsInRoleAsync(user As T, roleName As String, cancellationToken As CancellationToken) As Task(Of Boolean)
        Overloads Function GetUsersInRoleAsync(roleName As String, cancellationToken As CancellationToken) As Task(Of IList(Of T))
        'IUserEmailStore
        Overloads Function SetEmailAsync(user As T, email As String, cancellationToken As CancellationToken) As Task
        Overloads Function GetEmailAsync(user As T, cancellationToken As CancellationToken) As Task(Of String)
        Overloads Function GetEmailConfirmedAsync(user As T, cancellationToken As CancellationToken) As Task(Of Boolean)
        Overloads Function SetEmailConfirmedAsync(user As T, confirmed As Boolean, cancellationToken As CancellationToken) As Task
        Overloads Function FindByEmailAsync(normalizedEmail As String, cancellationToken As CancellationToken) As Task(Of T)
        Overloads Function GetNormalizedEmailAsync(user As T, cancellationToken As CancellationToken) As Task(Of String)
        Overloads Function SetNormalizedEmailAsync(user As T, normalizedEmail As String, cancellationToken As CancellationToken) As Task


    End Interface
End Namespace