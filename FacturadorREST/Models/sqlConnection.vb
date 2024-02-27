Imports System.Data.SqlClient

Public Class cnConnection

    Public Function con() As SqlConnection
        Dim strConn = Conexion()
        Dim cnCon = New SqlConnection(strConn)
        cnCon.Open()
        Return cnCon
    End Function

    Public Function conMarkeyWeb() As SqlConnection
        Dim strConn = ConexionMarkeyWeb()
        Dim cnCon = New SqlConnection(strConn)
        cnCon.Open()
        Return cnCon
    End Function

    Public Function Conexion() As String
        On Error Resume Next
        Err.Clear()

        Dim strServidor As String
        Dim strBase As String
        Dim strUsuario As String
        Dim strPassword As String

        Dim reader As New System.Configuration.AppSettingsReader
        strServidor = reader.GetValue("servidor", GetType(String))
        strBase = reader.GetValue("base", GetType(String))
        strUsuario = reader.GetValue("usuario", GetType(String))
        strPassword = Desencriptar(reader.GetValue("password", GetType(String)))


        Conexion = "Server=" & strServidor & ";Database=" & strBase & ";User ID=" & strUsuario & ";Password=" & strPassword & ";Trusted_Connection=False;"
        'Conexion = "Server=" & strServidor & ";Database=" & strBase & ";persist security info=True;Integrated Security=SSPI;"
    End Function

    Public Function ConexionMarkeyWeb() As String
        On Error Resume Next
        Err.Clear()

        Dim strServidor As String
        Dim strBase As String
        Dim strUsuario As String
        Dim strPassword As String

        Dim reader As New System.Configuration.AppSettingsReader
        strServidor = reader.GetValue("servidor2", GetType(String))
        strBase = reader.GetValue("base2", GetType(String))
        strUsuario = reader.GetValue("usuario2", GetType(String))
        strPassword = Desencriptar(reader.GetValue("password2", GetType(String)))

        ConexionMarkeyWeb = "Server=" & strServidor & ";Database=" & strBase & ";User ID=" & strUsuario & ";Password=" & strPassword & ";Trusted_Connection=False;"
    End Function
    Function Desencriptar(ByVal DataValue As Object) As Object
        On Error Resume Next
        Err.Clear()

        Dim x As Long
        Dim Temp As String = ""
        Dim HexByte As String

        For x = 1 To Len(DataValue) Step 2

            HexByte = Mid(DataValue, x, 2)
            Temp = Temp & Chr(ConvToInt(HexByte))

        Next x

        If Err.Number <> 0 Then
            Err.Clear()
            Desencriptar = ""
        Else
            Desencriptar = Temp
        End If

    End Function

    Private Function ConvToInt(ByVal x As String) As Integer

        Dim x1 As String
        Dim x2 As String
        Dim Temp As Integer

        x1 = Mid(x, 1, 1)
        x2 = Mid(x, 2, 1)

        If IsNumeric(x1) Then
            Temp = 16 * Int(x1)
        Else
            Temp = (Asc(x1) - 55) * 16
        End If

        If IsNumeric(x2) Then
            Temp = Temp + Int(x2)
        Else
            Temp = Temp + (Asc(x2) - 55)
        End If

        ' retorno
        ConvToInt = Temp

    End Function
    Public Function RemoteCertificateValidationCallback(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function

End Class
