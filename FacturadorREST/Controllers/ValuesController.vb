Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Description
Imports Newtonsoft.Json

Public Class ValuesController
    Inherits ApiController

    <HttpPost>
    <Route("ValorizarPracticas_000009CNS")>
    <ResponseType(GetType(FDH_VAL_Respuesta))>
    Public Function ValorizarPracticas_000009CNS(<FromBody> ByVal vFDH_VAL As List(Of FDH_VAL)) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim statuCode As HttpStatusCode

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter
        Dim lngProceso As Long

        Dim oFDH_VAL As FDH_VAL
        Dim oFDH_Estudio As FDH_VAL_Estudio

        Dim oFDH_VAL_Respuesta As New FDH_VAL_Respuesta
        Dim oFDH_VAL_Estudios_Respuesta As New List(Of FDH_VAL_Estudio_Respuesta)
        Dim oRespuesta As New EstadoRespuesta

        Dim oCon = New cnConnection
        Dim cnCon As SqlConnection = oCon.con()

        If cnCon.State = ConnectionState.Closed Then
            oRespuesta.CodigoRespuesta = 200
            oRespuesta.Mensaje = "Error al conectar con el SQL"

            oFDH_VAL_Respuesta.EstadoRespuesta = oRespuesta
            statuCode = HttpStatusCode.InternalServerError
            GoTo 1
        End If

        Dim cmdProceso As New SqlCommand
        With cmdProceso
            .Connection = cnCon
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ObtenerNumeroProceso"
            .Parameters.Add("@TIN_PRO_Interfaz", SqlDbType.VarChar).Value = "PracticasValorizacion"
            da = New SqlDataAdapter(cmdProceso)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            lngProceso = dt.Rows(0)("TIN_PRO_Numero")
        Else
            oRespuesta.CodigoRespuesta = 300
            oRespuesta.Mensaje = "Error al obtener el número de proceso."

            oFDH_VAL_Respuesta.EstadoRespuesta = oRespuesta
            statuCode = HttpStatusCode.NotFound
            GoTo 1
        End If

        For Each oFDH_VAL In vFDH_VAL
            For Each oFDH_Estudio In oFDH_VAL.FDH_VAL_Estudios

                If oFDH_VAL.FDH_VAL_AmbitoPractica <> "A" And oFDH_VAL.FDH_VAL_AmbitoPractica <> "H" And oFDH_VAL.FDH_VAL_AmbitoPractica <> "E" And oFDH_VAL.FDH_VAL_AmbitoPractica <> "U" Then
                    oRespuesta.CodigoRespuesta = 300
                    oRespuesta.Mensaje = "El valor del ámbito tiene que se A, H, E o U."

                    oFDH_VAL_Respuesta.EstadoRespuesta = oRespuesta
                    statuCode = HttpStatusCode.NotFound
                    GoTo 1
                End If

                If oFDH_Estudio.FDH_VAL_CantidadPractica < 1 Then
                    oRespuesta.CodigoRespuesta = 300
                    oRespuesta.Mensaje = "La cantidad debe ser mayor a 0."

                    oFDH_VAL_Respuesta.EstadoRespuesta = oRespuesta
                    statuCode = HttpStatusCode.NotFound
                    GoTo 1
                End If

                If oFDH_Estudio.FDH_VAL_CodigoPractica = "" Then
                    oRespuesta.CodigoRespuesta = 300
                    oRespuesta.Mensaje = "El código de práctica es obligatorio."

                    oFDH_VAL_Respuesta.EstadoRespuesta = oRespuesta
                    statuCode = HttpStatusCode.NotFound
                    GoTo 1
                End If

                If oFDH_Estudio.FDH_VAL_TipoPractica = "" Then
                    oRespuesta.CodigoRespuesta = 300
                    oRespuesta.Mensaje = "El tipo de práctica es obligatorio."

                    oFDH_VAL_Respuesta.EstadoRespuesta = oRespuesta
                    statuCode = HttpStatusCode.NotFound
                    GoTo 1
                End If

                Dim cmdGrabaPracticas As New SqlCommand
                With cmdGrabaPracticas
                    .Connection = cnCon
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "sp_GrabarPracticasValorizacion"
                    .Parameters.Add("@COD_CLIENT", SqlDbType.VarChar).Value = oFDH_VAL.FDH_VAL_IdPaciente
                    .Parameters.Add("@pers_CUIT", SqlDbType.VarChar).Value = oFDH_VAL.FDH_VAL_CuitFinan
                    .Parameters.Add("@plan_CodigoInterno", SqlDbType.VarChar).Value = oFDH_VAL.FDH_VAL_CodigoPlan
                    .Parameters.Add("@tcon_Codigo", SqlDbType.Int).Value = oFDH_VAL.FDH_VAL_CondicionPac
                    .Parameters.Add("@form_TipoFormulario", SqlDbType.VarChar).Value = oFDH_VAL.FDH_VAL_TipoFormu
                    .Parameters.Add("@form_NumeroFormulario", SqlDbType.VarChar).Value = oFDH_VAL.FDH_VAL_NumerFormu
                    .Parameters.Add("@form_Fecha", SqlDbType.DateTime).Value = oFDH_VAL.FDH_VAL_FechaPractica
                    .Parameters.Add("@form_Ambito", SqlDbType.VarChar).Value = oFDH_VAL.FDH_VAL_AmbitoPractica
                    .Parameters.Add("@empr_CUIT", SqlDbType.VarChar).Value = oFDH_VAL.FDH_VAL_Empresa
                    .Parameters.Add("@ubic_CodigoInterno", SqlDbType.VarChar).Value = oFDH_VAL.FDH_VAL_Sede
                    .Parameters.Add("@form_CPT", SqlDbType.VarChar).Value = oFDH_Estudio.FDH_VAL_CodigoPractica
                    .Parameters.Add("@form_Cantidad", SqlDbType.Int).Value = oFDH_Estudio.FDH_VAL_CantidadPractica
                    .Parameters.Add("@form_Correlativo", SqlDbType.Int).Value = oFDH_Estudio.FDH_VAL_CorrelativoPractica
                    .Parameters.Add("@prva_Proceso", SqlDbType.Int).Value = lngProceso

                    .ExecuteNonQuery()

                    If Err.Number <> 0 Then
                        oRespuesta.CodigoRespuesta = 500
                        oRespuesta.Mensaje = Err.Description

                        oFDH_VAL_Respuesta.EstadoRespuesta = oRespuesta
                        statuCode = HttpStatusCode.InternalServerError
                        GoTo 1
                    End If
                End With
            Next
        Next

        Dim cmdValorizacion As New SqlCommand
        With cmdValorizacion
            .Connection = cnCon
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ValorizarPracticasValorizacion"
            .Parameters.Add("@prva_Proceso", SqlDbType.Int).Value = lngProceso
            da = New SqlDataAdapter(cmdValorizacion)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For i = 1 To dt.Rows.Count - 1
                Dim oFDH_VAL_Estudio_Respuesta As New FDH_VAL_Estudio_Respuesta

                With oFDH_VAL_Respuesta
                    .FDH_VAL_IdPaciente = dt.Rows(i)("COD_CLIENT")
                    .FDH_VAL_CuitFinan = dt.Rows(i)("pers_CUIT")
                    .FDH_VAL_CodigoPlan = dt.Rows(i)("plan_CodigoInterno")
                    .FDH_VAL_CondicionPac = dt.Rows(i)("tcon_Codigo")
                    .FDH_VAL_Afiliado = dt.Rows(i)("paco_Afiliado")
                    .FDH_VAL_TipoFormu = dt.Rows(i)("form_TipoFormulario")
                    .FDH_VAL_NumerFormu = dt.Rows(i)("form_NumeroFormulario")
                    .FDH_VAL_FechaPractica = dt.Rows(i)("form_Fecha")
                    .FDH_VAL_AmbitoPractica = dt.Rows(i)("form_Ambito")
                    .FDH_VAL_Empresa = dt.Rows(i)("empr_CUIT")
                    .FDH_VAL_Sede = dt.Rows(i)("ubic_CodigoInterno")
                End With

                With oFDH_VAL_Estudio_Respuesta
                    .FDH_VAL_CodigoPractica = dt.Rows(i)("form_CPT")
                    .FDH_VAL_CantidadPractica = dt.Rows(i)("form_Cantidad")
                    .FDH_VAL_TipoPractica = dt.Rows(i)("form_TipoPractica")
                    .FDH_VAL_CorrelativoPractica = dt.Rows(i)("form_Correlativo")
                    .FDH_VAL_MontoTarif = dt.Rows(i)("FDH_VAL_MontoTarif")
                    .FDH_VAL_MontoNetoFinan = dt.Rows(i)("FDH_VAL_MontoNetoFinan")
                    .FDH_VAL_MontoIvaFinan = dt.Rows(i)("FDH_VAL_MontoIVAFinan")
                    .FDH_VAL_MontoTotalFinan = dt.Rows(i)("FDH_VAL_MontoTotalFinan")
                    .FDH_VAL_MontoNetoCopago = dt.Rows(i)("FDH_VAL_MontoNetoCopago")
                    .FDH_VAL_MontoIvaCopago = dt.Rows(i)("FDH_VAL_MontoIVACopago")
                    .FDH_VAL_MontoTotalCopago = dt.Rows(i)("FDH_VAL_MontoTotalCopago")
                    .FDH_VAL_MontoTotalBonoContribucion = dt.Rows(i)("FDH_VAL_MontoTotalBonoContribucion")
                    .FDH_VAL_PorcentajePaciente = dt.Rows(i)("FDH_VAL_PorcentajePaciente")
                    .FDH_VAL_PorcentajeCobertura = dt.Rows(i)("FDH_VAL_PorcentajeCobertura")
                    .FDH_VAL_RequiereAutorizacion = IIf(dt.Rows(i)("FDH_VAL_RequiereAutorizacion") = "S", True, False)
                End With
                oFDH_VAL_Estudios_Respuesta.Add(oFDH_VAL_Estudio_Respuesta)
            Next
            oFDH_VAL_Respuesta.FDH_VAL_Estudios_Respuesta = oFDH_VAL_Estudios_Respuesta

            oRespuesta.CodigoRespuesta = 0
            oRespuesta.Mensaje = "OK"
            oFDH_VAL_Respuesta.EstadoRespuesta = oRespuesta
            statuCode = HttpStatusCode.OK
            GoTo 1
        Else
            If Err.Number <> 0 Then
                oRespuesta.CodigoRespuesta = 100
                oRespuesta.Mensaje = Err.Description
                statuCode = HttpStatusCode.NotFound
                GoTo 1
            End If
        End If

        oFDH_VAL_Respuesta.EstadoRespuesta = oRespuesta

1:      If cnCon.State = ConnectionState.Open Then cnCon.Close()
        Return Request.CreateResponse(HttpStatusCode.OK, oFDH_VAL_Respuesta)

    End Function

    <HttpGet>
    <Route("obtenerPaciente")>
    <ResponseType(GetType(respuestaPaciente))>
    Public Function obtenerPaciente(Optional ByVal COD_CLIENT As String = "", Optional ByVal paci_HistoriaClinica As String = "", Optional ByVal pers_NumeroDocumento As String = "") As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New respuestaPaciente
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            Request.Headers.TryGetValues("Authorization", sToken)
            If sToken(0) <> sTokenValido Then
                oRespuesta.estado = "ERROR"
                oRespuesta.mensaje = "El token informádo no es válido - " & sToken(0)
                oRespuesta.ok = False
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
                Exit Function
            End If
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = "ERROR"
            oRespuesta.mensaje = "No se pudo conectar con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerPaciente"
            .Parameters.Add("@COD_CLIENT", SqlDbType.VarChar).Value = COD_CLIENT
            .Parameters.Add("@paci_HistoriaClinica", SqlDbType.VarChar).Value = paci_HistoriaClinica
            .Parameters.Add("@pers_NumeroDocumento", SqlDbType.VarChar).Value = pers_NumeroDocumento
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = "OK"
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oPaciente As New paciente
            With oPaciente
                .pers_Codigo = dt.Rows(0)("pers_Codigo")
                .pers_Apellido = dt.Rows(0)("pers_Apellido")
                .pers_Nombre = dt.Rows(0)("pers_Nombre")
                .pers_FechaNacimiento = dt.Rows(0)("pers_FechaNacimiento")
                .pers_Sexo = dt.Rows(0)("pers_Sexo")
                .pers_Pais = dt.Rows(0)("pers_Pais")
                .prov_Codigo = dt.Rows(0)("prov_Codigo")
                .pers_Calle = dt.Rows(0)("pers_Calle")
                .pers_Altura = dt.Rows(0)("pers_Altura")
                .pers_Piso = dt.Rows(0)("pers_Piso")
                .pers_Depto = dt.Rows(0)("pers_Depto")
                .pers_Localidad = dt.Rows(0)("pers_Localidad")
                .pers_CodigoPostal = dt.Rows(0)("pers_CodigoPostal")
                .pers_Mail = dt.Rows(0)("pers_Mail")
                .pers_Cuit = dt.Rows(0)("pers_Cuit")
                .tiva_Codigo = dt.Rows(0)("tiva_Codigo")
                .tper_Codigo = dt.Rows(0)("tper_Codigo")
                .tdoc_Codigo = dt.Rows(0)("tdoc_Codigo")
                .pers_NumeroDocumento = dt.Rows(0)("pers_NumeroDocumento")
                .COD_CLIENT = dt.Rows(0)("COD_CLIENT")
                .pers_FechaActualizacion = dt.Rows(0)("pers_FechaActualizacion")
                .pais_Codigo = dt.Rows(0)("pais_Codigo")
                .tper_Descripcion = dt.Rows(0)("tper_Descripcion")
                .tiva_Descripcion = dt.Rows(0)("tiva_Descripcion")
                .tdoc_Descripcion = dt.Rows(0)("tdoc_Descripcion")
                .paci_HistoriaClinica = dt.Rows(0)("paci_HistoriaClinica")
                .test_Codigo = dt.Rows(0)("test_Codigo")
                .paci_FechaGeneracion = dt.Rows(0)("paci_FechaGeneracion")
                .paci_ApellidoFacturacion = dt.Rows(0)("paci_ApellidoFacturacion")
                .paci_NombreFacturacion = dt.Rows(0)("paci_NombreFacturacion")
                .paci_CUITFacturacion = dt.Rows(0)("paci_CUITFacturacion")
                .paci_DomicilioFacturacion = dt.Rows(0)("paci_DomicilioFacturacion")
                .tiva_CodigoEntidad = dt.Rows(0)("tiva_CodigoEntidad")
                .tiva_DescripcionEntidad = dt.Rows(0)("tiva_DescripcionEntidad")
                .paci_CodigoNumerico = dt.Rows(0)("paci_CodigoNumerico")
            End With
            oRespuesta.paciente = oPaciente
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = "100"
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = "100"
                oRespuesta.mensaje = "No se pudo obtener la información del paciente."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerConvenio")>
    <ResponseType(GetType(EstadoRespuesta))>
    Public Function obtenerConvenio(CodigoConvenio As Long) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New EstadoRespuesta
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            Request.Headers.TryGetValues("Authorization", sToken)
            If sToken(0) <> sTokenValido Then
                oRespuesta.CodigoRespuesta = 500
                oRespuesta.Mensaje = "El token informádo no es válido - " & sToken(0)
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
                Exit Function
            End If
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.CodigoRespuesta = 500
            oRespuesta.Mensaje = "Error al conectar con el SQL"

            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If

        Dim oConMarkeyWeb = New cnConnection
        Dim oSQLMarkeyWeb As SqlConnection = oConMarkeyWeb.conMarkeyWeb()

        If oSQLMarkeyWeb.State = ConnectionState.Closed Then
            oRespuesta.CodigoRespuesta = 500
            oRespuesta.Mensaje = "Error al conectar con el SQL2"

            If oSQLMarkeyWeb.State = ConnectionState.Open Then oSQLMarkeyWeb.Close()
            If oSQL.State = ConnectionState.Open Then oSQL.Close()

            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerConvenio"
            .Parameters.Add("@conv_Codigo", SqlDbType.Int).Value = CodigoConvenio
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.CodigoRespuesta = 200
            oRespuesta.Mensaje = "Ok"

            Dim lOrden As Long

            For Each row In dt.Rows
                lOrden = lOrden + 1
                Dim cmdConvenio As New SqlCommand
                With cmdConvenio
                    .Connection = oSQLMarkeyWeb
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "apiGrabarConvenioPractica"
                    .Parameters.Add("@conv_Codigo", SqlDbType.Int).Value = row("conv_Codigo")
                    .Parameters.Add("@conv_Descripcion", SqlDbType.VarChar).Value = row("conv_Descripcion")
                    .Parameters.Add("@tnom_Codigo", SqlDbType.Int).Value = row("tnom_Codigo")
                    .Parameters.Add("@nome_Codigo", SqlDbType.VarChar).Value = row("nome_Codigo")
                    .Parameters.Add("@noho_Codigo", SqlDbType.VarChar).Value = row("noho_Codigo")
                    .Parameters.Add("@nome_Descripcion", SqlDbType.VarChar).Value = row("nome_Descripcion")
                    .Parameters.Add("@tpre_Descripcion", SqlDbType.VarChar).Value = row("tpre_Descripcion")
                    .Parameters.Add("@noco_Valor", SqlDbType.Decimal).Value = row("noco_Valor")
                    .Parameters.Add("@Orden", SqlDbType.Int).Value = lOrden
                    .ExecuteNonQuery()
                End With
            Next

            If oSQLMarkeyWeb.State = ConnectionState.Open Then oSQLMarkeyWeb.Close()
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oRespuesta.CodigoRespuesta = 100
                oRespuesta.Mensaje = Err.Description
                If oSQLMarkeyWeb.State = ConnectionState.Open Then oSQLMarkeyWeb.Close()
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.CodigoRespuesta = 100
                oRespuesta.Mensaje = "No se pudo obtener la información del paciente."
                If oSQLMarkeyWeb.State = ConnectionState.Open Then oSQLMarkeyWeb.Close()
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            End If
        End If
        If oSQLMarkeyWeb.State = ConnectionState.Open Then oSQLMarkeyWeb.Close()
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerConvenioPracticas")>
    <ResponseType(GetType(EstadoRespuesta))>
    Public Function obtenerConvenioPracticas(CodigoConvenio As Long) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New EstadoRespuesta
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            Request.Headers.TryGetValues("Authorization", sToken)
            If sToken(0) <> sTokenValido Then
                oRespuesta.CodigoRespuesta = 500
                oRespuesta.Mensaje = "El token informádo no es válido - " & sToken(0)
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
                Exit Function
            End If
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.CodigoRespuesta = 500
            oRespuesta.Mensaje = "Error al conectar con el SQL"

            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If

        Dim oConMarkeyWeb = New cnConnection
        Dim oSQLMarkeyWeb As SqlConnection = oConMarkeyWeb.conMarkeyWeb()

        If oSQLMarkeyWeb.State = ConnectionState.Closed Then
            oRespuesta.CodigoRespuesta = 500
            oRespuesta.Mensaje = "Error al conectar con el SQL2"

            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerConvenio"
            .Parameters.Add("@conv_Codigo", SqlDbType.Int).Value = CodigoConvenio
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.CodigoRespuesta = 200
            oRespuesta.Mensaje = "Ok"

            Dim oConvenioPracticas As New List(Of convenioPractica)
            For Each row In dt.Rows
                Dim oConvenioPractica As New convenioPractica
                With oConvenioPractica
                    .conv_Codigo = row("conv_Codigo")
                    .conv_Descripcion = row("conv_Descripcion")
                    .tnom_Codigo = row("tnom_Codigo")
                    .nome_Codigo = row("nome_Codigo")
                    .noho_Codigo = row("noho_Codigo")
                    .nome_Descripcion = row("nome_Descripcion")
                    .tpre_Descripcion = row("tpre_Descripcion")
                    .noco_Valor = row("noco_Valor")
                End With
                oConvenioPracticas.Add(oConvenioPractica)
            Next

            Dim json As String = JsonConvert.SerializeObject(oConvenioPracticas)

            Dim cmdConvenio As New SqlCommand
            With cmdConvenio
                .Connection = oSQLMarkeyWeb
                .CommandType = CommandType.StoredProcedure
                .CommandText = "apiGrabarConvenioPracticas"
                .Parameters.Add("@json", SqlDbType.VarChar).Value = json
                .ExecuteNonQuery()
            End With

            If oSQLMarkeyWeb.State = ConnectionState.Open Then oSQLMarkeyWeb.Close()
            If oSQL.State = ConnectionState.Open Then oSQL.Close()

            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oRespuesta.CodigoRespuesta = 100
                oRespuesta.Mensaje = Err.Description

                If oSQLMarkeyWeb.State = ConnectionState.Open Then oSQLMarkeyWeb.Close()
                If oSQL.State = ConnectionState.Open Then oSQL.Close()

                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.CodigoRespuesta = 100
                oRespuesta.Mensaje = "No se pudo obtener la información del paciente."

                If oSQLMarkeyWeb.State = ConnectionState.Open Then oSQLMarkeyWeb.Close()
                If oSQL.State = ConnectionState.Open Then oSQL.Close()

                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            End If
        End If
        If oSQLMarkeyWeb.State = ConnectionState.Open Then oSQLMarkeyWeb.Close()
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerOrdenes")>
    <ResponseType(GetType(hisTurnera.OrdenesMedicasDataWrapper))>
    Public Function obtenerOrdenes(ByVal CodigoPaciente As Long,
                                   Optional ByVal TipoOrden As String = "",
                                   Optional ByVal Ambito As String = "",
                                   Optional FechaDesde As Date = #01/01/1900#,
                                   Optional FechaHasta As Date = #01/01/2100#) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim wTurnera As New hisTurnera.TurneraServiceClient

        Dim oOrdenes As New hisTurnera.OrdenesMedicasDataWrapper
        oOrdenes = wTurnera.ObtenerOrdenesMedicas(CodigoPaciente, TipoOrden, Ambito, FechaDesde, FechaHasta)



        Return Request.CreateResponse(HttpStatusCode.InternalServerError, oOrdenes)
    End Function

    <HttpPost>
    <Route("ObtenerPacientes")>
    <Route("ObternerPacientes")>
    <ResponseType(GetType(respuestaPacienteTotem))>
    Public Function ObtenerPacientes(<FromBody> oNroDocumento As requestPacienteTotem) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oEstado As New estadoRespuestaTotem
        Dim oRespuesta As New respuestaPacienteTotem
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = "#ObtenerPacientes"
        Request.Headers.TryGetValues("SOAPAction", sToken)

        If sToken(0) <> sTokenValido Then
            oEstado.CodigoRespuesta = HttpStatusCode.Unauthorized
            oEstado.Mensaje = "El SOAPAction informado en el header no es válido - " & sToken(0)
            oRespuesta.EstadoRespuesta = oEstado
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oRespuesta)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
            oEstado.Mensaje = "Error al conectarse con la base de datos"
            oRespuesta.EstadoRespuesta = oEstado
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerPacienteDocumento"
            .Parameters.Add("@pers_NumeroDocumento", SqlDbType.VarChar).Value = oNroDocumento.NroDocumento
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oEstado.CodigoRespuesta = 200
            oEstado.Mensaje = "OK"
            Dim oPacientes As New List(Of pacienteTotem)
            For Each row In dt.Rows

                Dim oPaciente As New pacienteTotem
                With oPaciente
                    .Id = row("pers_Codigo")
                    .Apellido = row("pers_Apellido")
                    .Nombre = row("pers_Nombre")
                    .FechaNacimiento = row("pers_FechaNacimiento")
                    .Sexo = row("pers_Sexo")
                    .Pais = row("pers_Pais")
                    .ProvinciaCodigo = row("prov_Codigo")
                    .Calle = row("pers_Calle")
                    .Altura = row("pers_Altura")
                    .Piso = row("pers_Piso")
                    .Depto = row("pers_Depto")
                    .Localidad = row("pers_Localidad")
                    .CodigoPostal = row("pers_CodigoPostal")
                    .Mail = row("pers_Mail")
                    .TipoDocumentoCodigo = row("tdoc_Codigo")
                    .NroDocumento = row("pers_NumeroDocumento")
                    .PaisCodigo = row("pais_Codigo")
                    .TipoDocumento = row("tdoc_Descripcion")
                    .NroHistoriaClinica = row("paci_HistoriaClinica")
                    .AustralSalud = row("paci_AustralSalud")
                End With
                oPacientes.Add(oPaciente)
            Next
            oRespuesta.Pacientes = oPacientes
            oRespuesta.EstadoRespuesta = oEstado

            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
                oEstado.Mensaje = Err.Description
                oRespuesta.EstadoRespuesta = oEstado
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oEstado.CodigoRespuesta = HttpStatusCode.NotFound
                oEstado.Mensaje = "No se pudo obtener la información del paciente."
                oRespuesta.EstadoRespuesta = oEstado
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function


    <HttpGet>
    <Route("obtenerPacientesUnificado")>
    <ResponseType(GetType(respuestaPacientesUnificado))>
    Public Function obtenerPacientesUnificado(ByVal Filtro As String) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New respuestaPacientesUnificado
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oRespuesta.estado = HttpStatusCode.Unauthorized
            oRespuesta.mensaje = "El token informado en el header no es válido - " & sToken(0)
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oRespuesta)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = HttpStatusCode.InternalServerError
            oRespuesta.mensaje = "Error al conectarse con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerPacientesUnificado"
            .Parameters.Add("@Filtro", SqlDbType.VarChar).Value = Filtro
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = 200
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oPacientes As New List(Of paciente)
            For Each row In dt.Rows
                Dim oPaciente As New paciente
                With oPaciente
                    .pers_Codigo = row("pers_Codigo")
                    .pers_Apellido = row("pers_Apellido")
                    .pers_Nombre = row("pers_Nombre")
                    .pers_FechaNacimiento = row("pers_FechaNacimiento")
                    .pers_Sexo = row("pers_Sexo")
                    .pers_Pais = row("pers_Pais")
                    .prov_Codigo = row("prov_Codigo")
                    .pers_Calle = row("pers_Calle")
                    .pers_Altura = row("pers_Altura")
                    .pers_Piso = row("pers_Piso")
                    .pers_Depto = row("pers_Depto")
                    .pers_Localidad = row("pers_Localidad")
                    .pers_CodigoPostal = row("pers_CodigoPostal")
                    .pers_Mail = row("pers_Mail")
                    .pers_Cuit = row("pers_Cuit")
                    .tiva_Codigo = row("tiva_Codigo")
                    .tper_Codigo = row("tper_Codigo")
                    .tdoc_Codigo = row("tdoc_Codigo")
                    .pers_NumeroDocumento = row("pers_NumeroDocumento")
                    .COD_CLIENT = row("COD_CLIENT")
                    .pers_FechaActualizacion = row("pers_FechaActualizacion")
                    .pais_Codigo = row("pais_Codigo")
                    .tper_Descripcion = row("tper_Descripcion")
                    .tiva_Descripcion = row("tiva_Descripcion")
                    .tdoc_Descripcion = row("tdoc_Descripcion")
                    .paci_HistoriaClinica = row("paci_HistoriaClinica")
                    .test_Codigo = row("test_Codigo")
                    .paci_FechaGeneracion = row("paci_FechaGeneracion")
                    .paci_ApellidoFacturacion = row("paci_ApellidoFacturacion")
                    .paci_NombreFacturacion = row("paci_NombreFacturacion")
                    .paci_CUITFacturacion = row("paci_CUITFacturacion")
                    .paci_DomicilioFacturacion = row("paci_DomicilioFacturacion")
                    .tiva_CodigoEntidad = row("tiva_CodigoEntidad")
                    .tiva_DescripcionEntidad = row("tiva_DescripcionEntidad")
                    .paci_CodigoNumerico = row("paci_CodigoNumerico")
                End With

                Dim dtCoberturas As New DataTable
                Dim daCoberturas As New SqlDataAdapter

                Dim cmdCoberturas As New SqlCommand
                With cmdCoberturas
                    .Connection = oSQL
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "apiObtenerPacienteCobertura"
                    .Parameters.Add("@paci_CodigoNumerico", SqlDbType.Int).Value = oPaciente.paci_CodigoNumerico
                    daCoberturas = New SqlDataAdapter(cmdCoberturas)
                End With
                daCoberturas.Fill(dtCoberturas)
                If dtCoberturas.Rows.Count > 0 Then
                    Dim oCoberturas As New List(Of pacienteCobertura)
                    For Each rowC In dtCoberturas.Rows
                        Dim oCobertura As New pacienteCobertura
                        With oCobertura
                            .cobe_Codigo = rowC("cobe_Codigo")
                            .cobe_Descripcion = rowC("cobe_Descripcion")
                            .ivap_Porcentaje = rowC("ivap_Porcentaje")
                            .paci_CodigoNumerico = rowC("paci_CodigoNumerico")
                            .paco_Afiliado = rowC("paco_Afiliado")
                            .pers_Apellido = rowC("pers_Apellido")
                            .pers_Codigo = rowC("pers_Codigo")
                            .pers_CUIT = rowC("pers_Cuit")
                            .pers_Nombre = rowC("pers_Nombre")
                            .plan_Codigo = rowC("plan_Codigo")
                            .plan_CodigoInterno = rowC("plan_CodigoInterno")
                            .plan_Descripcion = rowC("plan_Descripcion")
                            .tcon_Codigo = rowC("tcon_Codigo")
                            .tcon_Descripcion = rowC("tcon_Descripcion")
                            .tcob_Codigo = rowC("tcob_Codigo")
                            .tcob_Descripcion = rowC("tcob_Descripcion")
                            .tcob_CodigoInterno = rowC("tcob_CodigoInterno")
                        End With
                        oCoberturas.Add(oCobertura)
                    Next
                    oPaciente.coberturas = oCoberturas
                End If
                oPacientes.Add(oPaciente)
            Next
            oRespuesta.pacientes = oPacientes
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = HttpStatusCode.InternalServerError
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = HttpStatusCode.NotFound
                oRespuesta.mensaje = "No se pudo obtener la información del paciente."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerPacienteCoberturas")>
    <ResponseType(GetType(respuestaPacienteCoberturas))>
    Public Function obtenerPacienteCoberturas(ByVal paci_CodigoNumerico As Long) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New respuestaPacienteCoberturas
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oRespuesta.estado = HttpStatusCode.Unauthorized
            oRespuesta.mensaje = "El token informado en el header no es válido - " & sToken(0)
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oRespuesta)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = HttpStatusCode.InternalServerError
            oRespuesta.mensaje = "Error al conectarse con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerPacienteCobertura"
            .Parameters.Add("@paci_CodigoNumerico", SqlDbType.Int).Value = paci_CodigoNumerico
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = 200
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oPacienteCoberturas As New List(Of pacienteCobertura)
            For Each rowC In dt.Rows
                Dim oPacienteCobertura As New pacienteCobertura
                With oPacienteCobertura
                    .cobe_Codigo = rowC("cobe_Codigo")
                    .cobe_Descripcion = rowC("cobe_Descripcion")
                    .ivap_Porcentaje = rowC("ivap_Porcentaje")
                    .paci_CodigoNumerico = rowC("paci_CodigoNumerico")
                    .paco_Afiliado = rowC("paco_Afiliado")
                    .pers_Apellido = rowC("pers_Apellido")
                    .pers_Codigo = rowC("pers_Codigo")
                    .pers_CUIT = rowC("pers_Cuit")
                    .pers_Nombre = rowC("pers_Nombre")
                    .plan_Codigo = rowC("plan_Codigo")
                    .plan_CodigoInterno = rowC("plan_CodigoInterno")
                    .plan_Descripcion = rowC("plan_Descripcion")
                    .tcon_Codigo = rowC("tcon_Codigo")
                    .tcon_Descripcion = rowC("tcon_Descripcion")
                    .tcob_Codigo = rowC("tcob_Codigo")
                    .tcob_Descripcion = rowC("tcob_Descripcion")
                    .tcob_CodigoInterno = rowC("tcob_CodigoInterno")
                End With
                oPacienteCoberturas.Add(oPacienteCobertura)
            Next
            oRespuesta.coberturas = oPacienteCoberturas
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = HttpStatusCode.InternalServerError
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = HttpStatusCode.NotFound
                oRespuesta.mensaje = "No se pudo obtener la información del paciente."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerCoberturas")>
    <ResponseType(GetType(respuestaCoberturas))>
    Public Function obtenerCoberturas(ByVal Optional cobe_Codigo As Long = 0) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New respuestaCoberturas
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oRespuesta.estado = HttpStatusCode.Unauthorized
            oRespuesta.mensaje = "El token informado en el header no es válido - " & sToken(0)
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oRespuesta)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = HttpStatusCode.InternalServerError
            oRespuesta.mensaje = "Error al conectarse con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerCobertura"
            If cobe_Codigo <> 0 Then
                .Parameters.Add("@cobe_Codigo", SqlDbType.Int).Value = cobe_Codigo
            End If
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = 200
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oCoberturas As New List(Of cobertura)
            For Each rowC In dt.Rows
                Dim oCobertura As New cobertura
                With oCobertura
                    .cobe_Codigo = rowC("cobe_Codigo")
                    .cobe_Descripcion = rowC("cobe_Descripcion")
                    .ivap_Porcentaje = rowC("ivap_Porcentaje")
                    .paco_Afiliado = rowC("paco_Afiliado")
                    .pers_Apellido = rowC("pers_Apellido")
                    .pers_Codigo = rowC("pers_Codigo")
                    .pers_CUIT = rowC("pers_Cuit")
                    .pers_Nombre = rowC("pers_Nombre")
                    .plan_Codigo = rowC("plan_Codigo")
                    .plan_CodigoInterno = rowC("plan_CodigoInterno")
                    .plan_Descripcion = rowC("plan_Descripcion")
                    .tcon_Codigo = rowC("tcon_Codigo")
                    .tcon_Descripcion = rowC("tcon_Descripcion")
                    .tcob_Codigo = rowC("tcob_Codigo")
                    .tcob_Descripcion = rowC("tcob_Descripcion")
                    .tcob_CodigoInterno = rowC("tcob_CodigoInterno")
                    .test_CodigoCobertura = rowC("test_CodigoCobertura")
                    .test_CodigoPlan = rowC("test_CodigoPlan")
                End With
                oCoberturas.Add(oCobertura)
            Next
            oRespuesta.coberturas = oCoberturas
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = HttpStatusCode.InternalServerError
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = HttpStatusCode.NotFound
                oRespuesta.mensaje = "No se pudo obtener la información del paciente."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerCPT")>
    <ResponseType(GetType(respuestaCPTs))>
    Public Function obtenerCPT(ByVal Optional cept_CodigoInterno As String = "") As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New respuestaCPTs
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oRespuesta.estado = HttpStatusCode.Unauthorized
            oRespuesta.mensaje = "El token informado en el header no es válido - " & sToken(0)
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oRespuesta)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = HttpStatusCode.InternalServerError
            oRespuesta.mensaje = "Error al conectarse con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerCPT"
            If cept_CodigoInterno <> "" Then
                .Parameters.Add("@cept_CodigoInterno", SqlDbType.VarChar).Value = cept_CodigoInterno
            End If
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = 200
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oCPTs As New List(Of cpt)
            For Each rowC In dt.Rows
                Dim oCPT As New cpt
                With oCPT
                    .cept_Agendable = rowC("cept_Agendable")
                    .cept_Codigo = rowC("cept_Codigo")
                    .cept_CodigoInterno = rowC("cept_CodigoInterno")
                    .cept_Consulta = rowC("cept_Consulta")
                    .cept_Descripcion = rowC("cept_Descripcion")
                    .cept_HospitalDia = rowC("cept_HospitalDia")
                    .cept_Tiempo = rowC("cept_Tiempo")
                    .cgru_Codigo = rowC("cgru_Codigo")
                    .cgru_Descripcion = rowC("cgru_Descripcion")
                    .csub_Codigo = rowC("csub_Codigo")
                    .csub_Descripcion = rowC("csub_Descripcion")
                    .test_CodigoCPT = rowC("test_CodigoCPT")
                End With
                oCPTs.Add(oCPT)
            Next
            oRespuesta.cpts = oCPTs
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = HttpStatusCode.InternalServerError
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = HttpStatusCode.NotFound
                oRespuesta.mensaje = "No se pudo obtener la información del paciente."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerNomencladorNacional")>
    <ResponseType(GetType(respuestaNomencladorNacional))>
    Public Function obtenerNomencladorNacional(ByVal Optional nome_Codigo As String = "") As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New respuestaNomencladorNacional
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oRespuesta.estado = HttpStatusCode.Unauthorized
            oRespuesta.mensaje = "El token informado en el header no es válido - " & sToken(0)
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oRespuesta)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = HttpStatusCode.InternalServerError
            oRespuesta.mensaje = "Error al conectarse con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerNomencladorNacional"
            If nome_Codigo <> "" Then
                .Parameters.Add("@nome_Codigo", SqlDbType.VarChar).Value = nome_Codigo
            End If
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = 200
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oNNs As New List(Of nomencladorNacional)
            For Each rowC In dt.Rows
                Dim oNN As New nomencladorNacional
                With oNN
                    .mone_Codigo = rowC("mone_Codigo")
                    .mone_Denominacion = rowC("mone_Denominacion")
                    .mone_Descripcion = rowC("mone_Descripcion")
                    .mone_Simbolo = rowC("mone_Simbolo")
                    .nome_Codigo = rowC("nome_Codigo")
                    .nome_CodigoHomologado = rowC("nome_CodigoHomologado")
                    .nome_Descripcion = rowC("nome_Descripcion")
                    .nome_Estado = rowC("nome_Estado")
                    .nome_Nomenclador = rowC("nome_Nomenclador")
                    .nome_Procesamiento = rowC("nome_Procesamiento")
                    .nome_SinCargo = rowC("nome_SinCargo")
                    .nome_SubDescripcion = rowC("nome_SubDescripcion")
                    .tnom_Codigo = rowC("tnom_Codigo")
                    .tnom_Descripcion = rowC("tnom_Descripcion")
                    .tpre_Abreviatura = rowC("tpre_Abreviatura")
                    .tpre_Codigo = rowC("tpre_Codigo")
                    .tpre_Descricpion = rowC("tpre_Descricpion")
                End With
                oNNs.Add(oNN)
            Next
            oRespuesta.nomencladorNacional = oNNs
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = HttpStatusCode.InternalServerError
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = HttpStatusCode.NotFound
                oRespuesta.mensaje = "No se pudo obtener la información del paciente."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerCPTNomenclador")>
    <ResponseType(GetType(respuestaCptNomenclador))>
    Public Function obtenerCPTNomenclador(ByVal Optional cept_CodigoInterno As String = "") As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New respuestaCptNomenclador
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oRespuesta.estado = HttpStatusCode.Unauthorized
            oRespuesta.mensaje = "El token informado en el header no es válido - " & sToken(0)
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oRespuesta)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = HttpStatusCode.InternalServerError
            oRespuesta.mensaje = "Error al conectarse con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerCPTNomenclador"
            If cept_CodigoInterno <> "" Then
                .Parameters.Add("@cept_CodigoInterno", SqlDbType.VarChar).Value = cept_CodigoInterno
            End If
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = 200
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oNNs As New List(Of cptNomenclador)
            For Each rowC In dt.Rows
                Dim oNN As New cptNomenclador
                With oNN
                    .cobe_Codigo = rowC("cobe_Codigo")
                    .cpet_Codigo = rowC("cpet_Codigo")
                    .cptn_Cantidad = rowC("cptn_Cantidad")
                    .nome_Codigo = rowC("nome_Codigo")
                    .tnom_Codigo = rowC("tnom_Codigo")
                End With
                oNNs.Add(oNN)
            Next
            oRespuesta.cptNomencladores = oNNs
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = HttpStatusCode.InternalServerError
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = HttpStatusCode.NotFound
                oRespuesta.mensaje = "No se pudo obtener la información del paciente."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function


    <HttpPost>
    <Route("ObtenerTurnos")>
    <Route("totem/getTurnos")>
    <ResponseType(GetType(List(Of turno)))>
    <ResponseType(GetType(EstadoRespuesta))>
    Public Function ObtenerTurnos(<FromBody> oFiltroTurnos As turnoFiltro) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oEstado As New EstadoRespuesta
        Dim oRespuesta As New List(Of turno)

        'Dim reader As New System.Configuration.AppSettingsReader

        'Dim sToken() As String, sTokenValido As String

        'sTokenValido = "#ObtenerPacientes"
        'Request.Headers.TryGetValues("SOAPAction", sToken)

        'If sToken(0) <> sTokenValido Then
        '    oEstado.CodigoRespuesta = HttpStatusCode.Unauthorized
        '    oEstado.Mensaje = "El SOAPAction informado en el header no es válido - " & sToken(0)
        '    oRespuesta.EstadoRespuesta = oEstado
        '    Return Request.CreateResponse(HttpStatusCode.Unauthorized, oRespuesta)
        '    Exit Function
        'End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
            oEstado.Mensaje = "Error al conectarse con la base de datos"
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerTurnosPaciente"
            .Parameters.Add("@COD_CLIENT", SqlDbType.VarChar).Value = oFiltroTurnos.Id
            .Parameters.Add("@papr_FechaDesde", SqlDbType.DateTime).Value = oFiltroTurnos.Desde
            .Parameters.Add("@papr_FechaHasta", SqlDbType.DateTime).Value = oFiltroTurnos.Hasta
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oEstado.CodigoRespuesta = 200
            oEstado.Mensaje = "OK"
            Dim oTurnos As New List(Of turno)
            For Each row In dt.Rows

                Dim oTurno As New turno
                With oTurno
                    .Id = row("Id")
                    .Fecha = row("Fecha")
                    .FechaInicio = row("FechaInicio")
                    .FechaFin = row("FechaFin")
                    .SobreTurno = row("SobreTurno")
                    .Medico = row("Medico")
                    .Especialidad = row("Especialidad")
                    .Servicio = row("Servicio")
                    .IdPaciente = row("IdPaciente")
                    .Equipo = row("Equipo")
                    .Cobertura = row("Cobertura")
                    .Ubicacion = row("Ubicacion")
                    .IdEstado = row("IdEstado")
                    .EstadoTurno = row("EstadoTurno")
                    .IdConsultorio = row("IdConsultorio")
                    .Consultorio = row("Consultorio")
                    .Llegada = row("Llegada")
                    .IdProcedimiento = row("IdProcedimiento")
                    .Procedimiento = row("Procedimiento")
                    .CodigoAdmision = row("CodigoAdmision")
                    .CodigoSala = row("CodigoSala")
                    .CodigoConsultorio = row("CodigoConsultorio")
                    .autoAdmision = row("autoAdmision")
                    .Afiliado = row("Afiliado")
                    .CUITFinanciador = row("CUITFinanciador")
                    .CodigoPaciente = row("CodigoPaciente")
                    .CodigoPractica = row("CodigoPractica")
                    .SolicitaCodigoSeguridad = row("SolicitaCodigoSeguridad")
                    .TipoFormulario = row("TipoFormulario")
                    .NumeroFormulario = row("NumeroFormulario")
                    .TipoPractica = row("TipoPractica")
                    .Paciente = row("Paciente")
                End With
                oTurnos.Add(oTurno)
            Next
            oRespuesta = oTurnos
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
                oEstado.Mensaje = Err.Description
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
            Else
                oEstado.CodigoRespuesta = HttpStatusCode.NotFound
                oEstado.Mensaje = "No se pudo obtener la información del paciente."
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oEstado)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpPost>
    <Route("informarGestionTurno")>
    <ResponseType(GetType(EstadoRespuesta))>
    Public Function informarGestionTurno(<FromBody> oGestionTurno As informarGestionTurno) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oEstado As New EstadoRespuesta

        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oEstado.CodigoRespuesta = HttpStatusCode.Unauthorized
            oEstado.Mensaje = "El token informado en el header no es válido"
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oEstado)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
            oEstado.Mensaje = "Error al conectarse con la base de datos"
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiInformarGestionTurno"
            .Parameters.Add("@papr_TipoFormulario", SqlDbType.VarChar).Value = oGestionTurno.tipoFormulario
            .Parameters.Add("@papr_NumeroFormulario", SqlDbType.VarChar).Value = oGestionTurno.numeroFormulario
            .Parameters.Add("@papr_FechaEnvio", SqlDbType.DateTime).Value = oGestionTurno.fechaEnvio
            .Parameters.Add("@papr_FechaGestion", SqlDbType.DateTime).Value = oGestionTurno.fechaGestion
            .Parameters.Add("@papr_ResultadoGestion", SqlDbType.VarChar).Value = oGestionTurno.resultadoGestion
            .Parameters.Add("@papr_CodigoGestion", SqlDbType.VarChar).Value = oGestionTurno.codigo
            .Parameters.Add("@papr_Observaciones", SqlDbType.VarChar).Value = oGestionTurno.observaciones
            .ExecuteNonQuery()
        End With
        If Err.Number <> 0 Then
            oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
            oEstado.Mensaje = Err.Description
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
        Else
            oEstado.CodigoRespuesta = HttpStatusCode.OK
            oEstado.Mensaje = "OK"
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oEstado)
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpPost>
    <Route("informarPracticaAnatomia")>
    <ResponseType(GetType(EstadoRespuesta))>
    Public Function informarPracticaAnatomia(<FromBody> sHL7 As mensajeHL7) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oEstado As New EstadoRespuesta

        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oEstado.CodigoRespuesta = HttpStatusCode.Unauthorized
            oEstado.Mensaje = "El token informado en el header no es válido"
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oEstado)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
            oEstado.Mensaje = "Error al conectarse con la base de datos"
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiInformarPracticaAnatomia"
            .Parameters.Add("@HL7", SqlDbType.VarChar).Value = sHL7.mensaje
            .ExecuteNonQuery()
        End With
        If Err.Number <> 0 Then
            oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
            oEstado.Mensaje = Err.Description
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
        Else
            oEstado.CodigoRespuesta = HttpStatusCode.OK
            oEstado.Mensaje = "OK"
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oEstado)
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerTurnosRecordatorio")>
    <ResponseType(GetType(List(Of turnoRecordatorio)))>
    <ResponseType(GetType(EstadoRespuesta))>
    Public Function obtenerTurnosRecordatorio(<FromBody> oFiltroTurnos As turnoRecordatorioFiltro) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oEstado As New EstadoRespuesta
        Dim oRespuesta As New List(Of turnoRecordatorio)

        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oEstado.CodigoRespuesta = HttpStatusCode.Unauthorized
            oEstado.Mensaje = "El token informado en el header no es válido"
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oEstado)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
            oEstado.Mensaje = "Error al conectarse con la base de datos"
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiTurnoRecordatorio"
            .Parameters.Add("@papr_FechaDesde", SqlDbType.DateTime).Value = oFiltroTurnos.fechaDesde
            .Parameters.Add("@papr_FechaHasta", SqlDbType.DateTime).Value = oFiltroTurnos.fechaHasta
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oEstado.CodigoRespuesta = 200
            oEstado.Mensaje = "OK"
            Dim oTurnos As New List(Of turnoRecordatorio)
            For Each row In dt.Rows

                Dim oTurno As New turnoRecordatorio
                With oTurno
                    .codigoCpt = row("cept_CodigoInterno")
                    .codigoPaciente = row("COD_CLIENT")
                    .codigoPractica = row("nome_Codigo")
                    .codigoServicio = row("form_CodigoServicio")
                    .descripcionCpt = row("cept_Descripcion")
                    .descripcionPractica = row("nome_Descripcion")
                    .descripcionServicio = row("form_DescripcionServicio")
                    .fechaTurno = row("papr_Fecha")
                    .historiaClinica = row("paci_HistoriaClinica")
                    .numeroDocumento = row("pers_NumeroDocumento")
                    .numeroFormulario = row("papr_NumeroFormulario")
                    .paciente = row("paci_Paciente")
                    .pideConfirmacion = row("turn_PideConfirmacion")
                    .profesional = row("medi_Medico")
                    .sede = row("ubic_Descripcion")
                    .telefono1 = row("tele_Telefono1")
                    .telefono2 = row("tele_Telefono2")
                    .tipoFormulario = row("papr_TipoFormulario")
                    .cuitCobertura = row("cobe_CUIT")
                    .descripcionCobertura = row("cobe_Descripcion")
                    .razonSocialCobertura = row("cobe_RazonSocial")
                    .codigoPlan = row("plan_CodigoInterno")
                    .descripcionPlan = row("plan_Descripcion")
                End With
                oTurnos.Add(oTurno)
            Next
            oRespuesta = oTurnos
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
                oEstado.Mensaje = Err.Description
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
            Else
                oEstado.CodigoRespuesta = HttpStatusCode.NotFound
                oEstado.Mensaje = "No se pudo obtener la información del paciente."
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oEstado)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerDatosTurno")>
    <ResponseType(GetType(datosTurno))>
    <ResponseType(GetType(EstadoRespuesta))>
    Public Function obtenerTurnosRecordatorio(<FromBody> oTurno As obtenerDatosTurnoFiltro) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oEstado As New EstadoRespuesta
        Dim oRespuesta As New datosTurno

        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            oEstado.CodigoRespuesta = HttpStatusCode.Unauthorized
            oEstado.Mensaje = "El token informado en el header no es válido"
            Return Request.CreateResponse(HttpStatusCode.Unauthorized, oEstado)
            Exit Function
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
            oEstado.Mensaje = "Error al conectarse con la base de datos"
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
            Exit Function
        End If

        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerDatosTurno"
            .Parameters.Add("@COD_CLIENT", SqlDbType.VarChar).Value = oTurno.codigoPaciente
            .Parameters.Add("@papr_TipoFormulario", SqlDbType.VarChar).Value = oTurno.tipoFormulario
            .Parameters.Add("@papr_NumeroFormulario", SqlDbType.VarChar).Value = oTurno.numeroFormulario
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oEstado.CodigoRespuesta = 200
            oEstado.Mensaje = "OK"

            Dim oTurnoRespuesta As New datosTurno
            With oTurnoRespuesta
                .tipoFormulario = dt.Rows(0)("papr_TipoFormulario")
                .numeroFormulario = dt.Rows(0)("papr_NumeroFormulario")
                .pacienteConsultorio = dt.Rows(0)("papr_Consultorio")
                .pacienteEstadoRecepcion = dt.Rows(0)("papr_EstadoRecepcion")
                .pacienteLlegada = dt.Rows(0)("papr_Llegada")
                .pacienteNumero = dt.Rows(0)("COD_CLIENT")
                .pacienteRecepcion = dt.Rows(0)("papr_Recepcion")
                .pacienteSalida = dt.Rows(0)("papr_Salida")
            End With

            oRespuesta = oTurnoRespuesta
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oRespuesta)
        Else
            If Err.Number <> 0 Then
                oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
                oEstado.Mensaje = Err.Description
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
            Else
                oEstado.CodigoRespuesta = HttpStatusCode.NotFound
                oEstado.Mensaje = "No se pudo obtener la información del paciente."
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.NotFound, oEstado)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerUbicaciones")>
    <ResponseType(GetType(List(Of internacion.ubicacion)))>
    <ResponseType(GetType(internacion.respuesta))>
    Public Function obtenerUbicaciones(Optional ByVal reprocesar As Boolean = False) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New internacion.respuesta
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            Request.Headers.TryGetValues("Authorization", sToken)
            If sToken(0) <> sTokenValido Then
                oRespuesta.estado = "ERROR"
                oRespuesta.mensaje = "El token informádo no es válido - " & sToken(0)
                oRespuesta.ok = False
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
                Exit Function
            End If
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = "ERROR"
            oRespuesta.mensaje = "No se pudo conectar con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerEntidades"
            .Parameters.Add("@enti_Entidad", SqlDbType.VarChar).Value = "Ubicaciones"
            .Parameters.Add("@enti_Reprocesar", SqlDbType.Bit).Value = reprocesar
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = "OK"
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oUbicaciones As New List(Of internacion.ubicacion)
            For Each row In dt.Rows
                Dim oUbicacion As New internacion.ubicacion
                With oUbicacion
                    .ubic_Altura = row("ubic_Altura")
                    .ubic_Calle = row("ubic_Calle")
                    .ubic_Codigo = row("ubic_Codigo")
                    .ubic_CodigoHPGD = row("ubic_CodigoHPGD")
                    .ubic_CodigoInterno = row("ubic_CodigoInterno")
                    .ubic_CodigoPostal = row("ubic_CodigoPostal")
                    .ubic_CodigoProvincia = row("ubic_CodigoProvincia")
                    .ubic_Descripcion = row("ubic_Descripcion")
                    .ubic_Localidad = row("ubic_Localidad")
                    .ubic_PartidoCodigoProvincia = row("ubic_PartidoCodigoProvincia")
                    .ubic_RegionSanitaria = row("ubic_RegionSanitaria")
                End With
                oUbicaciones.Add(oUbicacion)
            Next
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oUbicaciones)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = "100"
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = "100"
                oRespuesta.mensaje = "No se encontraron novedades."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerSala")>
    <ResponseType(GetType(List(Of internacion.sala)))>
    <ResponseType(GetType(internacion.respuesta))>
    Public Function obtenerSala(Optional ByVal reprocesar As Boolean = False) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New internacion.respuesta
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            Request.Headers.TryGetValues("Authorization", sToken)
            If sToken(0) <> sTokenValido Then
                oRespuesta.estado = "ERROR"
                oRespuesta.mensaje = "El token informádo no es válido - " & sToken(0)
                oRespuesta.ok = False
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
                Exit Function
            End If
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = "ERROR"
            oRespuesta.mensaje = "No se pudo conectar con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerEntidades"
            .Parameters.Add("@enti_Entidad", SqlDbType.VarChar).Value = "Sala"
            .Parameters.Add("@enti_Reprocesar", SqlDbType.Bit).Value = reprocesar
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = "OK"
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oSalas As New List(Of internacion.sala)
            For Each row In dt.Rows
                Dim oSala As New internacion.sala
                With oSala
                    .ubic_Codigo = row("ubic_Codigo")
                    .sala_Codigo = row("sala_Codigo")
                    .sala_Descripcion = row("sala_Descripcion")
                    .sala_CodigoInterno = row("sala_CodigoInterno")
                End With
                oSalas.Add(oSala)
            Next
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oSalas)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = "100"
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = "100"
                oRespuesta.mensaje = "No se encontraron novedades."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerArea")>
    <ResponseType(GetType(List(Of internacion.area)))>
    <ResponseType(GetType(internacion.respuesta))>
    Public Function obtenerArea(Optional ByVal reprocesar As Boolean = False) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New internacion.respuesta
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            Request.Headers.TryGetValues("Authorization", sToken)
            If sToken(0) <> sTokenValido Then
                oRespuesta.estado = "ERROR"
                oRespuesta.mensaje = "El token informádo no es válido - " & sToken(0)
                oRespuesta.ok = False
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
                Exit Function
            End If
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = "ERROR"
            oRespuesta.mensaje = "No se pudo conectar con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerEntidades"
            .Parameters.Add("@enti_Entidad", SqlDbType.VarChar).Value = "Area"
            .Parameters.Add("@enti_Reprocesar", SqlDbType.Bit).Value = reprocesar
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = "OK"
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oAreas As New List(Of internacion.area)
            For Each row In dt.Rows
                Dim oArea As New internacion.area
                With oArea
                    .ubic_Codigo = row("ubic_Codigo")
                    .sala_Codigo = row("sala_Codigo")
                    .area_Codigo = row("area_Codigo")
                    .unfu_Codigo = row("unfu_Codigo")
                    .depo_Codigo = row("depo_Codigo")
                    .area_Descripcion = row("area_Descripcion")
                    .area_CodigoInterno = row("area_CodigoInterno")
                End With
                oAreas.Add(oArea)
            Next
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oAreas)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = "100"
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = "100"
                oRespuesta.mensaje = "No se encontraron novedades."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerCama")>
    <ResponseType(GetType(List(Of internacion.cama)))>
    <ResponseType(GetType(internacion.respuesta))>
    Public Function obtenerCama(Optional ByVal reprocesar As Boolean = False) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New internacion.respuesta
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            Request.Headers.TryGetValues("Authorization", sToken)
            If sToken(0) <> sTokenValido Then
                oRespuesta.estado = "ERROR"
                oRespuesta.mensaje = "El token informádo no es válido - " & sToken(0)
                oRespuesta.ok = False
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
                Exit Function
            End If
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = "ERROR"
            oRespuesta.mensaje = "No se pudo conectar con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerEntidades"
            .Parameters.Add("@enti_Entidad", SqlDbType.VarChar).Value = "Cama"
            .Parameters.Add("@enti_Reprocesar", SqlDbType.Bit).Value = reprocesar
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = "OK"
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oCamas As New List(Of internacion.cama)
            For Each row In dt.Rows
                Dim oCama As New internacion.cama
                With oCama
                    .ubic_Codigo = row("ubic_Codigo")
                    .sala_Codigo = row("sala_Codigo")
                    .area_Codigo = row("area_Codigo")
                    .cama_Codigo = row("cama_Codigo")
                    .teca_Codigo = row("teca_Codigo")
                    .sepe_Codigo = row("sepe_Codigo")
                    .cama_Descripcion = row("cama_Descripcion")
                    .cama_CodigoInterno = row("cama_CodigoInterno")
                    .cama_NroInterno = row("cama_NroInterno")
                End With
                oCamas.Add(oCama)
            Next
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oCamas)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = "100"
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = "100"
                oRespuesta.mensaje = "No se encontraron novedades."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpGet>
    <Route("obtenerInternaciones")>
    <ResponseType(GetType(List(Of internacion.internacion)))>
    <ResponseType(GetType(internacion.respuesta))>
    Public Function obtenerInternaciones(Optional ByVal paci_CodigoNumerico As Long = 0, Optional ByVal inte_FechaDesde As Date = #01/01/1900#, Optional ByVal inte_FechaHasta As Date = #01/01/1900#, Optional ByVal reprocesar As Boolean = False) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oRespuesta As New internacion.respuesta
        Dim reader As New System.Configuration.AppSettingsReader
        Dim sToken() As String, sTokenValido As String

        sTokenValido = reader.GetValue("token", GetType(String))
        Request.Headers.TryGetValues("Token", sToken)

        If sToken(0) <> sTokenValido Then
            Request.Headers.TryGetValues("Authorization", sToken)
            If sToken(0) <> sTokenValido Then
                oRespuesta.estado = "ERROR"
                oRespuesta.mensaje = "El token informádo no es válido - " & sToken(0)
                oRespuesta.ok = False
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
                Exit Function
            End If
        End If

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.estado = "ERROR"
            oRespuesta.mensaje = "No se pudo conectar con la base de datos"
            oRespuesta.ok = False
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Exit Function
        End If


        Dim cmd As New SqlCommand
        With cmd
            .Connection = oSQL
            .CommandType = CommandType.StoredProcedure
            .CommandText = "apiObtenerInternaciones"
            If paci_CodigoNumerico <> 0 Then .Parameters.Add("@paci_CodigoNumerico", SqlDbType.Int).Value = paci_CodigoNumerico
            .Parameters.Add("@inte_FechaDesde", SqlDbType.DateTime).Value = inte_FechaDesde
            .Parameters.Add("@inte_FechaHasta", SqlDbType.DateTime).Value = inte_FechaHasta
            .Parameters.Add("@inte_Reprocesar", SqlDbType.Bit).Value = reprocesar
            da = New SqlDataAdapter(cmd)
        End With
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            oRespuesta.estado = "OK"
            oRespuesta.mensaje = "OK"
            oRespuesta.ok = True
            Dim oInternaciones As New List(Of internacion.internacion)
            For Each row In dt.Rows
                Dim oInternacion As New internacion.internacion
                With oInternacion
                    .ubic_Codigo = row("ubic_Codigo")
                    .inte_Codigo = row("inte_Codigo")
                    .inte_Ano = row("inte_Ano")
                    .inte_FechaIngreso = row("inte_FechaIngreso")
                    .inte_FechaEgreso = row("inte_FechaEgreso")
                    .inte_Observacion = row("inte_Observacion")
                    .tint_Codigo = row("tint_Codigo")
                    .tint_Descripcion = row("tint_Descripcion")
                    .talt_Codigo = row("talt_Codigo")
                    .talt_Descripcion = row("talt_Descripcion")
                    .paci_CodigoNumerico = row("paci_CodigoNumerico")
                    .pers_Codigo = row("pers_Codigo")
                    .COD_CLIENT = row("COD_CLIENT")
                    .cobe_Codigo = row("cobe_Codigo")
                    .plan_Codigo = row("plan_Codigo")
                    .plan_CodigoInterno = row("plan_CodigoInterno")
                    .tcon_Codigo = row("tcon_Codigo")
                    .tcon_Descripcion = row("tcon_Descripcion")
                    .tcom_Codigo = row("tcom_Codigo")
                    .tcom_Descripcion = row("tcom_Descripcion")
                End With
                oInternaciones.Add(oInternacion)
            Next
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.OK, oInternaciones)
        Else
            If Err.Number <> 0 Then
                oRespuesta.estado = "100"
                oRespuesta.mensaje = Err.Description
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            Else
                oRespuesta.estado = "100"
                oRespuesta.mensaje = "No se encontraron novedades."
                oRespuesta.ok = False
                If oSQL.State = ConnectionState.Open Then oSQL.Close()
                Return Request.CreateResponse(HttpStatusCode.InternalServerError, oRespuesta)
            End If
        End If
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
    End Function

    <HttpPost>
    <Route("PlanificarCirugia")>
    <ResponseType(GetType(EstadoRespuesta))>
    Public Function PlanificarCirugia(<FromBody> oCirugia As quirofano.planificacionCirugia) As HttpResponseMessage
        'On Error Resume Next
        'Err.Clear()

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim dt As New DataTable
        Dim da As New SqlDataAdapter

        Dim oEstado As New EstadoRespuesta
        Dim oRespuesta As New hisQuirofano.EstadoRespuesta

        Dim wsHIS As New hisQuirofano.OrdenesMedicasServiceClient

        With oCirugia
            oRespuesta = wsHIS.PlanificarCirugia_000082ABM(.codigoPaciente, .tipoOrden, .numeroOrden, .nroCasoBPM, .fechaPlanificada, .suspendido, .cancelado, .qxAsignado, .motivo, .usuarioNovedad, .comentarios)
        End With

        If Err.Number <> 0 Then
            oEstado.CodigoRespuesta = HttpStatusCode.InternalServerError
            oEstado.Mensaje = Err.Description
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oEstado)
        Else
            oEstado.CodigoRespuesta = oRespuesta.CodigoRespuesta
            oEstado.Mensaje = oRespuesta.Mensaje
            Return Request.CreateResponse(HttpStatusCode.OK, oEstado)
        End If
    End Function

    <HttpPost>
    <Route("ObtenerModuloCPT_000032CNS")>
    <ResponseType(GetType(ModuloPrestacion_Respuesta))>
    Public Function ObtenerModuloCPT_000032CNS(<FromBody> ByVal oModuloPrestacion As ModuloPrestacion) As HttpResponseMessage
        On Error Resume Next
        Err.Clear()

        Dim i As Integer
        Dim strMensaje As String

        Dim oModulos As New List(Of Modulo)

        Dim oRespuesta As New EstadoRespuesta
        Dim oModuloPrestacion_Respuesta As New ModuloPrestacion_Respuesta

        oRespuesta.CodigoRespuesta = 0
        oRespuesta.Mensaje = ""

        Dim oCon = New cnConnection
        Dim oSQL As SqlConnection = oCon.con()

        If oSQL.State = ConnectionState.Closed Then
            oRespuesta.CodigoRespuesta = 500
            oRespuesta.Mensaje = "No se pudo conectar con la base de datos"
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, oModuloPrestacion_Respuesta)
            Exit Function
        End If

        If oModuloPrestacion.CUITFinanciador = "" Or IsNothing(oModuloPrestacion.CUITFinanciador) Then
            oRespuesta.CodigoRespuesta = 300
            oRespuesta.Mensaje = "El CUIT del Financiador es un campo requerido."

            oModuloPrestacion_Respuesta.EstadoRespuesta = oRespuesta
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.NotFound, oModuloPrestacion_Respuesta)
            Exit Function
        End If

        If oModuloPrestacion.CodigoPlanFinanciador = "" Or IsNothing(oModuloPrestacion.CodigoPlanFinanciador) Then
            oRespuesta.CodigoRespuesta = 300
            oRespuesta.Mensaje = "El código de plan del Financiador es un campo requerido."

            oModuloPrestacion_Respuesta.EstadoRespuesta = oRespuesta
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.NotFound, oModuloPrestacion_Respuesta)
            Exit Function
        End If

        If oModuloPrestacion.Ambito = "" Or IsNothing(oModuloPrestacion.Ambito) Then
            oModuloPrestacion.Ambito = "H"
        End If

        If oModuloPrestacion.CodigoSede = "" Or IsNothing(oModuloPrestacion.CodigoSede) Then
            oModuloPrestacion.CodigoSede = 1
        End If

        If oModuloPrestacion.CUITEmpresa = "" Or IsNothing(oModuloPrestacion.CUITEmpresa) Then
            oModuloPrestacion.CUITEmpresa = ""
        End If

        If oModuloPrestacion.CodigoCPT.Count = 0 Then
            oRespuesta.CodigoRespuesta = 300
            oRespuesta.Mensaje = "El código de CPT es un campo requerido."

            oModuloPrestacion_Respuesta.EstadoRespuesta = oRespuesta
            If oSQL.State = ConnectionState.Open Then oSQL.Close()
            Return Request.CreateResponse(HttpStatusCode.NotFound, oModuloPrestacion_Respuesta)
            Exit Function
        End If

        strMensaje = ""

        For Each oCPT In oModuloPrestacion.CodigoCPT
            Dim dt As New DataTable
            Dim da As New SqlDataAdapter

            Dim cmd As New SqlCommand
            With cmd
                .Connection = oSQL
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_ObtenerModuloPrestacion_000032CNS"
                .Parameters.Add("@CUITFinanciador", SqlDbType.VarChar).Value = oModuloPrestacion.CUITFinanciador
                .Parameters.Add("@CodigoPlanFinanciador", SqlDbType.VarChar).Value = oModuloPrestacion.CodigoPlanFinanciador
                .Parameters.Add("@Ambito", SqlDbType.VarChar).Value = oModuloPrestacion.Ambito
                .Parameters.Add("@CodigoSede", SqlDbType.VarChar).Value = oModuloPrestacion.CodigoSede
                .Parameters.Add("@CUITEmpresa", SqlDbType.VarChar).Value = oModuloPrestacion.CUITEmpresa
                .Parameters.Add("@CodigoCPT", SqlDbType.VarChar).Value = oCPT
                .Parameters.Add("@FechaOrden", SqlDbType.DateTime).Value = oModuloPrestacion.FechaOrden
                da = New SqlDataAdapter(cmd)
            End With
            oSQL.Close()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Dim oModulo As New Modulo
                With oModulo
                    .CodigoCPT = oCPT
                    .DescripcionCPT = dt(0)("DescripcionCPT")
                    .CodigoPractica = dt(0)("CodigoPractica")
                    .DescripcionPractica = dt(0)("CodigoPractica")
                    .CodigoModulo = dt(0)("CodigoModulo")
                    .DescripcionModulo = dt(0)("DescripcionModulo")
                    .CodigoPracticaHomologado = dt(0)("CodigoPracticaHomologado")
                    .CodigoModuloHomologado = dt(0)("CodigoModuloHomologado")
                    .PracticaAutorizacion = IIf(dt(0)("PracticaAutorizacion") = "N", False, True)
                    .ModuloAutorizacion = IIf(dt(0)("ModuloAutorizacionN") = "N", False, True)
                    .PracticaConvenida = IIf(dt(0)("PracticaConvenida") = "N", False, True)
                    .ModuloConvenido = IIf(dt(0)("ModuloConvenido") = "N", False, True)
                    .MarcaPresupuesto = IIf(dt(0)("MarcaPresupuesto") = "N", False, True)
                    .ImportePaciente = dt(0)("ImportePaciente")
                    .ImporteCobertura = dt(0)("ImporteCobertura")
                    .EstadoCPT = dt(0)("EstadoCPT")
                End With

                oModulos.Add(oModulo)

                oRespuesta.CodigoRespuesta = 0
                oRespuesta.Mensaje = "OK"
            Else
                If Err.Number <> 0 Then
                    oRespuesta.CodigoRespuesta = 100
                    oRespuesta.Mensaje = Err.Description
                    If oSQL.State = ConnectionState.Open Then oSQL.Close()
                    Return Request.CreateResponse(HttpStatusCode.InternalServerError, oModuloPrestacion_Respuesta)
                    Exit Function
                Else
                    Dim oModulo As New Modulo
                    With oModulo
                        .CodigoCPT = oCPT
                        .EstadoCPT = "CPT INEXISTENTE"
                    End With

                    oModulos.Add(oModulo)

                    oRespuesta.CodigoRespuesta = 0
                    oRespuesta.Mensaje = "OK"
                End If
            End If
        Next

        oModuloPrestacion_Respuesta.CodigosCPTs = oModulos
        oModuloPrestacion_Respuesta.EstadoRespuesta = oRespuesta
        If oSQL.State = ConnectionState.Open Then oSQL.Close()
        Return Request.CreateResponse(HttpStatusCode.OK, oModuloPrestacion_Respuesta)
    End Function
End Class
