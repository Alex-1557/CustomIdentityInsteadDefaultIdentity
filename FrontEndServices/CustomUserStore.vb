Imports System.Text
Imports System.Threading
Imports BackendAPI.Jwt
Imports BackendAPI.Model
Imports FrontEndData.Models
Imports Microsoft.AspNetCore.Identity
Imports Microsoft.Extensions.Logging
Imports Microsoft.Extensions.Options
Imports BackendAPI.Helper
Imports Microsoft.AspNetCore.Http

Namespace Services
    Public Class CustomUserStore
        Implements ICustomUserStore(Of ApplicationUser)


        Private ReadOnly _HttpContextAccessor As IHttpContextAccessor
        Private  _DB As ApplicationDbContext
        Private _UserList As List(Of ApplicationUser)
        Private disposedValue1 As Boolean
        Private disposedValue As Boolean
        Private ReadOnly _Log As ILogger(Of CustomUserStore)
        Private ReadOnly _UserDecryptPass As String
        Private ReadOnly RequestID As Integer

        Public Sub New(DbContext As ApplicationDbContext, ByVal AppSettings As IOptions(Of JwtSettings), Logger As ILogger(Of CustomUserStore), HttpContextAccessor As IHttpContextAccessor)
            _UserList = New List(Of ApplicationUser)
            _DB = DbContext
            _Log = Logger
            _HttpContextAccessor = HttpContextAccessor
            _UserDecryptPass = Encoding.UTF8.GetString(Convert.FromBase64String("XXXXXXXXXXXXXX")) 'https://www.base64decode.org/  
            RefreshUsers()
            _UserList.ForEach(Sub(X) If X.DbPassword IsNot Nothing Then X.Password = Encoding.UTF8.GetString(X.DbPassword))
            RequestID = HttpContextAccessor.HttpContext.GetHashCode
        End Sub
        Sub RefreshUsers()
            _UserList = _DB.RawSqlQuery(Of ApplicationUser)($"Select i,Name,IsAdmin, AES_DECRYPT(Password,'{_UserDecryptPass}') as Password from `User`;",
                                                Function(X) New ApplicationUser With {
                                                .UserName = X.Item("Name"),
                                                .Id = X.Item("i"),
                                                .IsAdmin = X.CheckDBNull(Of Integer)(X.Item("IsAdmin")),
                                                .DbPassword = X.CheckDBNull(Of Byte())(X.Item("Password"))
                                                 }).Item1
        End Sub

#Region "IUserStore"

        'Login - 3, Register - 8
        Private Function IUserStore_GetUserIdAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements IUserStore(Of ApplicationUser).GetUserIdAsync
            _Log.LogInformation($"{RequestID}: IUserStore_GetUserIdAsync")
            Return Task.FromResult(User.Id.ToString)
        End Function
        'Login - 4, Register - 4, Manage -2
        Private Function IUserStore_GetUserNameAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements IUserStore(Of ApplicationUser).GetUserNameAsync
            _Log.LogInformation($"{RequestID}: IUserStore_GetUserNameAsync")
            Return Task.FromResult(User.UserName)
        End Function
        'Register - 1
        Private Function IUserStore_SetUserNameAsync(User As ApplicationUser, userName As String, cancellationToken As CancellationToken) As Task Implements IUserStore(Of ApplicationUser).SetUserNameAsync
            _Log.LogInformation($"{RequestID}: IUserStore_SetUserNameAsync")
            User.UserName = userName
            Return Task.CompletedTask
        End Function

        Private Function IUserStore_GetNormalizedUserNameAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements IUserStore(Of ApplicationUser).GetNormalizedUserNameAsync
            _Log.LogInformation($"{RequestID}: IUserStore_GetNormalizedUserNameAsync")
            Throw New NotImplementedException()
        End Function
        'Register - 5
        Private Function IUserStore_SetNormalizedUserNameAsync(User As ApplicationUser, normalizedName As String, cancellationToken As CancellationToken) As Task Implements IUserStore(Of ApplicationUser).SetNormalizedUserNameAsync
            _Log.LogInformation($"{RequestID}: IUserStore_SetNormalizedUserNameAsync")
            Return Task.CompletedTask
        End Function
        'Register - 7
        Private Async Function IUserStore_CreateAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of IdentityResult) Implements IUserStore(Of ApplicationUser).CreateAsync
            _Log.LogInformation($"{RequestID}: IUserStore_CreateAsync")
            'Dim Max_I As Integer = Sql.ExecRDR(Of Integer)(_DB, "SELECT MAX(I) as Max_I  FROM cryptochestmax.User;", Function(X) X("Max_I"))(0)
            Await Sql.ExecNonQueryAsync(_DB, $"INSERT INTO `cryptochestmax`.`User`(`Name`,`Password`,`IsAdmin`,`LastUpdate`) VALUES('{User.UserName}', AES_ENCRYPT('{User.Password}','{_UserDecryptPass}'),0,Now()); ")
            RefreshUsers()
            Return IdentityResult.Success
        End Function

        Private Async Function IUserStore_UpdateAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of IdentityResult) Implements IUserStore(Of ApplicationUser).UpdateAsync
            _Log.LogInformation($"{RequestID}: IUserStore_UpdateAsync")
            Return IdentityResult.Success
        End Function

        Private Function IUserStore_DeleteAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of IdentityResult) Implements IUserStore(Of ApplicationUser).DeleteAsync
            _Log.LogInformation($"{RequestID}: IUserStore_DeleteAsync")
            Throw New NotImplementedException()
        End Function
        'Manage -1
        Private Function IUserStore_FindByIdAsync(userId As String, cancellationToken As CancellationToken) As Task(Of ApplicationUser) Implements IUserStore(Of ApplicationUser).FindByIdAsync
            _Log.LogInformation($"{RequestID}: IUserStore_FindByIdAsync")
            Dim CurUser As ApplicationUser = _UserList.Where(Function(X) X.Id = userId).FirstOrDefault

            Return Task.FromResult(CurUser)
        End Function
        'Login - 1, Register - 5
        Private Function IUserStore_FindByNameAsync(normalizedUserName As String, cancellationToken As CancellationToken) As Task(Of ApplicationUser) Implements IUserStore(Of ApplicationUser).FindByNameAsync
            _Log.LogInformation($"{RequestID}: IUserStore_FindByNameAsync")
            Dim CurUser = _UserList.Where(Function(x) x.UserName.ToLower = normalizedUserName.ToLower).FirstOrDefault
            If CurUser IsNot Nothing Then
                Return Task.FromResult(CurUser)
            Else
                Return Task.FromResult(TryCast(Nothing, ApplicationUser))
            End If
        End Function
#End Region

#Region "ICustomUserStore"

        Public Function ICustomUserStore_GetUsersInRoleAsync(roleName As String, cancellationToken As CancellationToken) As Task(Of IList(Of BackendAPI.Model.ApplicationUser)) Implements ICustomUserStore(Of ApplicationUser).GetUsersInRoleAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_GetUsersInRoleAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_SetPasswordHashAsync(user As BackendAPI.Model.ApplicationUser, passwordHash As String, cancellationToken As CancellationToken) As Task Implements ICustomUserStore(Of ApplicationUser).SetPasswordHashAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_SetPasswordHashAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_GetPasswordHashAsync(user As BackendAPI.Model.ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements ICustomUserStore(Of ApplicationUser).GetPasswordHashAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_GetPasswordHashAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_HasPasswordAsync(user As BackendAPI.Model.ApplicationUser, cancellationToken As CancellationToken) As Task(Of Boolean) Implements ICustomUserStore(Of ApplicationUser).HasPasswordAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_HasPasswordAsync")
            Throw New NotImplementedException()
        End Function

        Private Function ICustomUserStore_AddToRoleAsync(User As ApplicationUser, roleName As String, cancellationToken As CancellationToken) As Task Implements ICustomUserStore(Of ApplicationUser).AddToRoleAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_AddToRoleAsync")
            Throw New NotImplementedException()
        End Function

        Private Function ICustomUserStore_RemoveFromRoleAsync(User As ApplicationUser, roleName As String, cancellationToken As CancellationToken) As Task Implements ICustomUserStore(Of ApplicationUser).RemoveFromRoleAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_RemoveFromRoleAsync")
            Throw New NotImplementedException()
        End Function

        Private Function ICustomUserStore_GetRolesAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of IList(Of String)) Implements ICustomUserStore(Of ApplicationUser).GetRolesAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_GetRolesAsync")
            Throw New NotImplementedException()
        End Function

        Private Function ICustomUserStore_IsInRoleAsync(User As ApplicationUser, roleName As String, cancellationToken As CancellationToken) As Task(Of Boolean) Implements ICustomUserStore(Of ApplicationUser).IsInRoleAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_IsInRoleAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_SetEmailAsync(user As ApplicationUser, email As String, cancellationToken As CancellationToken) As Task Implements ICustomUserStore(Of ApplicationUser).SetEmailAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_SetEmailAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_GetEmailAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements ICustomUserStore(Of ApplicationUser).GetEmailAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_GetEmailAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_GetEmailConfirmedAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of Boolean) Implements ICustomUserStore(Of ApplicationUser).GetEmailConfirmedAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_GetEmailConfirmedAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_SetEmailConfirmedAsync(user As ApplicationUser, confirmed As Boolean, cancellationToken As CancellationToken) As Task Implements ICustomUserStore(Of ApplicationUser).SetEmailConfirmedAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_SetEmailConfirmedAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_FindByEmailAsync(normalizedEmail As String, cancellationToken As CancellationToken) As Task(Of ApplicationUser) Implements ICustomUserStore(Of ApplicationUser).FindByEmailAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_FindByEmailAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_GetNormalizedEmailAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements ICustomUserStore(Of ApplicationUser).GetNormalizedEmailAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_GetNormalizedEmailAsync")
            Throw New NotImplementedException()
        End Function

        Public Function ICustomUserStore_SetNormalizedEmailAsync(user As ApplicationUser, normalizedEmail As String, cancellationToken As CancellationToken) As Task Implements ICustomUserStore(Of ApplicationUser).SetNormalizedEmailAsync
            _Log.LogInformation($"{RequestID}: ICustomUserStore_SetNormalizedEmailAsync")
            Throw New NotImplementedException()
        End Function
#End Region

#Region "IUserPasswordStore"
        'Register - 3
        Private Function IUserPasswordStore_SetPasswordHashAsync(User As ApplicationUser, passwordHash As String, cancellationToken As CancellationToken) As Task Implements IUserPasswordStore(Of ApplicationUser).SetPasswordHashAsync
            'Hash https://stackoverflow.com/questions/20621950/asp-net-identitys-default-password-hasher-how-does-it-work-and-is-it-secure
            Dim Login As String = _HttpContextAccessor.HttpContext.Request.Form("Input.Email")(0)
            Dim ClearPassword As String = _HttpContextAccessor.HttpContext.Request.Form("Input.Password")(0)
            User.Password = ClearPassword
            Return Task.CompletedTask
        End Function
        'Login - 2
        Private Function IUserPasswordStore_GetPasswordHashAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements IUserPasswordStore(Of ApplicationUser).GetPasswordHashAsync
            Dim Hash = New PasswordHasher(Of ApplicationUser)().HashPassword(User, User.Password)
            Return Task.FromResult(Hash)
        End Function
        ' Manage - Password
        Private Function IUserPasswordStore_HasPasswordAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of Boolean) Implements IUserPasswordStore(Of ApplicationUser).HasPasswordAsync
            Return Task.FromResult(True)
        End Function
#End Region

#Region "IUserRoleStore"
        Public Function IUserRoleStore_AddToRoleAsync(User As ApplicationUser, roleName As String, cancellationToken As CancellationToken) As Task Implements IUserRoleStore(Of ApplicationUser).AddToRoleAsync
            Throw New NotImplementedException()
        End Function

        Public Function IUserRoleStore_RemoveFromRoleAsync(User As ApplicationUser, roleName As String, cancellationToken As CancellationToken) As Task Implements IUserRoleStore(Of ApplicationUser).RemoveFromRoleAsync
            Throw New NotImplementedException()
        End Function
        'Login - 1,5
        Public Function IUserRoleStore_GetRolesAsync(User As ApplicationUser, cancellationToken As CancellationToken) As Task(Of IList(Of String)) Implements IUserRoleStore(Of ApplicationUser).GetRolesAsync
            Dim Roles As IList(Of String) = New List(Of String)
            If User.IsAdmin Then
                Roles.Add("Admin")
            Else
                Roles.Add("User")
            End If
            Return Task.FromResult(Roles)
        End Function

        Public Function IUserRoleStore_IsInRoleAsync(User As ApplicationUser, roleName As String, cancellationToken As CancellationToken) As Task(Of Boolean) Implements IUserRoleStore(Of ApplicationUser).IsInRoleAsync
            Throw New NotImplementedException()
        End Function

        Private Function IUserRoleStore_GetUsersInRoleAsync(roleName As String, cancellationToken As CancellationToken) As Task(Of IList(Of ApplicationUser)) Implements IUserRoleStore(Of ApplicationUser).GetUsersInRoleAsync
            Throw New NotImplementedException()
        End Function
#End Region

#Region "IUserEmailStore"

        'Register - 2
        Private Function IUserEmailStore_SetEmailAsync(user As ApplicationUser, email As String, cancellationToken As CancellationToken) As Task Implements IUserEmailStore(Of ApplicationUser).SetEmailAsync
            Return Task.CompletedTask
        End Function
        ''Register - 5, Manage - Email
        Private Function IUserEmailStore_GetEmailAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements IUserEmailStore(Of ApplicationUser).GetEmailAsync
            Return Task.FromResult($"Hello {user.UserName}")
        End Function
        ' Manage - Email
        Private Function IUserEmailStore_GetEmailConfirmedAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of Boolean) Implements IUserEmailStore(Of ApplicationUser).GetEmailConfirmedAsync
            Return Task.FromResult(False)
        End Function

        Private Function IUserEmailStore_SetEmailConfirmedAsync(user As ApplicationUser, confirmed As Boolean, cancellationToken As CancellationToken) As Task Implements IUserEmailStore(Of ApplicationUser).SetEmailConfirmedAsync
            Throw New NotImplementedException()
        End Function

        Private Function IUserEmailStore_FindByEmailAsync(normalizedEmail As String, cancellationToken As CancellationToken) As Task(Of ApplicationUser) Implements IUserEmailStore(Of ApplicationUser).FindByEmailAsync
            Throw New NotImplementedException()
        End Function

        Private Function IUserEmailStore_GetNormalizedEmailAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements IUserEmailStore(Of ApplicationUser).GetNormalizedEmailAsync
            Throw New NotImplementedException()
        End Function
        'Register - 7
        Private Function IUserEmailStore_SetNormalizedEmailAsync(user As ApplicationUser, normalizedEmail As String, cancellationToken As CancellationToken) As Task Implements IUserEmailStore(Of ApplicationUser).SetNormalizedEmailAsync
            Return Task.CompletedTask
        End Function

#End Region

#Region "IUserPhoneNumberStore"

        Public Function IUserPhoneNumberStore_SetPhoneNumberAsync(user As ApplicationUser, phoneNumber As String, cancellationToken As CancellationToken) As Task Implements IUserPhoneNumberStore(Of ApplicationUser).SetPhoneNumberAsync
            _Log.LogInformation($"{RequestID}: IUserPhoneNumberStore_SetPhoneNumberAsync")
            Throw New NotImplementedException()
        End Function
        ' Manage=3
        Public Function IUserPhoneNumberStore_GetPhoneNumberAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements IUserPhoneNumberStore(Of ApplicationUser).GetPhoneNumberAsync
            _Log.LogInformation($"{RequestID}: IUserPhoneNumberStore_GetPhoneNumberAsync")
            Return Task.FromResult("No phone set up")
        End Function

        Public Function IUserPhoneNumberStore_GetPhoneNumberConfirmedAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of Boolean) Implements IUserPhoneNumberStore(Of ApplicationUser).GetPhoneNumberConfirmedAsync
            _Log.LogInformation($"{RequestID}: IUserPhoneNumberStore_GetPhoneNumberConfirmedAsync")
            Return Task.FromResult(False)
        End Function

        Public Function IUserPhoneNumberStore_SetPhoneNumberConfirmedAsync(user As ApplicationUser, confirmed As Boolean, cancellationToken As CancellationToken) As Task Implements IUserPhoneNumberStore(Of ApplicationUser).SetPhoneNumberConfirmedAsync
            _Log.LogInformation($"{RequestID}: IUserPhoneNumberStore_SetPhoneNumberConfirmedAsync")
            Throw New NotImplementedException()
        End Function

#End Region

#Region "IUserAuthenticatorKeyStore - 2FactorAU"
        Public Function IUserAuthenticatorKeyStore_SetAuthenticatorKeyAsync(user As ApplicationUser, key As String, cancellationToken As CancellationToken) As Task Implements IUserAuthenticatorKeyStore(Of ApplicationUser).SetAuthenticatorKeyAsync
            _Log.LogInformation($"{RequestID}: IUserAuthenticatorKeyStore_SetAuthenticatorKeyAsync")
            Throw New NotImplementedException()
        End Function
        ' Manage - Two-factor authentication 1
        Public Function IUserAuthenticatorKeyStore_GetAuthenticatorKeyAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of String) Implements IUserAuthenticatorKeyStore(Of ApplicationUser).GetAuthenticatorKeyAsync
            _Log.LogInformation($"{RequestID}: IUserAuthenticatorKeyStore_GetAuthenticatorKeyAsync")
            Return Task.FromResult(user.Id.ToString)
        End Function
#End Region

#Region "IUserTwoFactorStore"
        Public Function SetTwoFactorEnabledAsync(user As ApplicationUser, enabled As Boolean, cancellationToken As CancellationToken) As Task Implements IUserTwoFactorStore(Of ApplicationUser).SetTwoFactorEnabledAsync
            Throw New NotImplementedException()
        End Function
        ' Manage - Two-factor authentication 2
        Public Function GetTwoFactorEnabledAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of Boolean) Implements IUserTwoFactorStore(Of ApplicationUser).GetTwoFactorEnabledAsync
            Return Task.FromResult(False)
        End Function
#End Region

#Region "IUserTwoFactorRecoveryCodeStore"
        Public Function IUserTwoFactorRecoveryCodeStore_ReplaceCodesAsync(user As ApplicationUser, recoveryCodes As IEnumerable(Of String), cancellationToken As CancellationToken) As Task Implements IUserTwoFactorRecoveryCodeStore(Of ApplicationUser).ReplaceCodesAsync
            _Log.LogInformation($"{RequestID}: IUserTwoFactorRecoveryCodeStore_ReplaceCodesAsync")
            Throw New NotImplementedException()
        End Function

        Public Function IUserTwoFactorRecoveryCodeStore_RedeemCodeAsync(user As ApplicationUser, code As String, cancellationToken As CancellationToken) As Task(Of Boolean) Implements IUserTwoFactorRecoveryCodeStore(Of ApplicationUser).RedeemCodeAsync
            _Log.LogInformation($"{RequestID}: IUserTwoFactorRecoveryCodeStore_RedeemCodeAsync")
            Throw New NotImplementedException()
        End Function
        ' Manage - Two-factor authentication 3
        Public Function IUserTwoFactorRecoveryCodeStore_CountCodesAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of Integer) Implements IUserTwoFactorRecoveryCodeStore(Of ApplicationUser).CountCodesAsync
            _Log.LogInformation($"{RequestID}: IUserTwoFactorRecoveryCodeStore_CountCodesAsync")
            Return Task.FromResult(1)
        End Function
#End Region

#Region "IUserLoginStore"
        Public Function IUserLoginStore_AddLoginAsync(user As ApplicationUser, login As UserLoginInfo, cancellationToken As CancellationToken) As Task Implements IUserLoginStore(Of ApplicationUser).AddLoginAsync
            _Log.LogInformation($"{RequestID}: IUserLoginStore_AddLoginAsync")
            Throw New NotImplementedException()
        End Function

        Public Function IUserLoginStore_RemoveLoginAsync(user As ApplicationUser, loginProvider As String, providerKey As String, cancellationToken As CancellationToken) As Task Implements IUserLoginStore(Of ApplicationUser).RemoveLoginAsync
            _Log.LogInformation($"{RequestID}: IUserLoginStore_RemoveLoginAsync")
            Throw New NotImplementedException()
        End Function

        Public Function IUserLoginStore_GetLoginsAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of IList(Of UserLoginInfo)) Implements IUserLoginStore(Of ApplicationUser).GetLoginsAsync
            _Log.LogInformation($"{RequestID}: IUserLoginStore_GetLoginsAsync")
            'Dim OwinContext As IOwinContext = CType(_HttpContextAccessor.HttpContext.Request, IOwinContext)
            'If OwinContext IsNot Nothing Then
            '    'Owin.Get("")
            'End If
            Dim List1 As IList(Of UserLoginInfo) = New List(Of UserLoginInfo)
            Return Task.FromResult(List1)
        End Function

        Public Function IUserLoginStore_FindByLoginAsync(loginProvider As String, providerKey As String, cancellationToken As CancellationToken) As Task(Of ApplicationUser) Implements IUserLoginStore(Of ApplicationUser).FindByLoginAsync
            _Log.LogInformation($"{RequestID}: IUserLoginStore_FindByLoginAsync")
            Throw New NotImplementedException()
        End Function
#End Region


#Region "IUserLockoutStore"

        Public Function IUserLockoutStore_GetLockoutEndDateAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of DateTimeOffset?) Implements IUserLockoutStore(Of ApplicationUser).GetLockoutEndDateAsync
            _Log.LogInformation($"{RequestID}: IUserLockoutStore_GetLockoutEndDateAsync")
            Throw New NotImplementedException()
        End Function

        Public Function IUserLockoutStore_SetLockoutEndDateAsync(user As ApplicationUser, lockoutEnd As DateTimeOffset?, cancellationToken As CancellationToken) As Task Implements IUserLockoutStore(Of ApplicationUser).SetLockoutEndDateAsync
            _Log.LogInformation($"{RequestID}: IUserLockoutStore_SetLockoutEndDateAsync")
            Throw New NotImplementedException()
        End Function

        Public Function IUserLockoutStore_IncrementAccessFailedCountAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of Integer) Implements IUserLockoutStore(Of ApplicationUser).IncrementAccessFailedCountAsync
            _Log.LogInformation($"{RequestID}: IUserLockoutStore_IncrementAccessFailedCountAsync")
            Return Task.FromResult(0)
        End Function

        Public Function IUserLockoutStore_ResetAccessFailedCountAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task Implements IUserLockoutStore(Of ApplicationUser).ResetAccessFailedCountAsync
            _Log.LogInformation($"{RequestID}: IUserLockoutStore_ResetAccessFailedCountAsync")
            Throw New NotImplementedException()
        End Function

        Public Function IUserLockoutStore_GetAccessFailedCountAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of Integer) Implements IUserLockoutStore(Of ApplicationUser).GetAccessFailedCountAsync
            _Log.LogInformation($"{RequestID}: IUserLockoutStore_GetAccessFailedCountAsync")
            Return Task.FromResult(0)
        End Function

        Public Function IUserLockoutStore_GetLockoutEnabledAsync(user As ApplicationUser, cancellationToken As CancellationToken) As Task(Of Boolean) Implements IUserLockoutStore(Of ApplicationUser).GetLockoutEnabledAsync
            _Log.LogInformation($"{RequestID}: IUserLockoutStore_GetLockoutEnabledAsync")
            Return Task.FromResult(False)
        End Function

        Public Function IUserLockoutStore_SetLockoutEnabledAsync(user As ApplicationUser, enabled As Boolean, cancellationToken As CancellationToken) As Task Implements IUserLockoutStore(Of ApplicationUser).SetLockoutEnabledAsync
            _Log.LogInformation($"{RequestID}: IUserLockoutStore_SetLockoutEnabledAsync")
            Throw New NotImplementedException()
        End Function

#End Region



#Region "Dispose"
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects)
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ' TODO: set large fields to null
                disposedValue = True
            End If
        End Sub

        ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
        ' Protected Overrides Sub Finalize()
        '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region


    End Class
End Namespace