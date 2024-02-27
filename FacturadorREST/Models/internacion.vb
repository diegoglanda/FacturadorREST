Public Class internacion
    Public Class internacion
        Public Property ubic_Codigo As Integer
        Public Property inte_Codigo As Long
        Public Property inte_Ano As Integer
        Public Property inte_FechaIngreso As Date
        Public Property inte_FechaEgreso As Date
        Public Property inte_Observacion As String
        Public Property tint_Codigo As Integer
        Public Property tint_Descripcion As String
        Public Property talt_Codigo As Integer
        Public Property talt_Descripcion As String
        Public Property paci_CodigoNumerico As Long
        Public Property pers_Codigo As Long
        Public Property COD_CLIENT As String
        Public Property cobe_Codigo As Long
        Public Property plan_Codigo As Long
        Public Property plan_CodigoInterno As String
        Public Property tcon_Codigo As Integer
        Public Property tcon_Descripcion As String
        Public Property tcom_Codigo As Integer
        Public Property tcom_Descripcion As String
    End Class

    Public Class ubicacion
        Public Property ubic_Codigo As Integer
        Public Property ubic_Descripcion As String
        Public Property ubic_Calle As String
        Public Property ubic_Altura As Integer
        Public Property ubic_Localidad As String
        Public Property ubic_CodigoPostal As String
        Public Property ubic_PartidoCodigoProvincia As String
        Public Property ubic_CodigoInterno As String
        Public Property ubic_CodigoProvincia As String
        Public Property ubic_CodigoHPGD As String
        Public Property ubic_RegionSanitaria As String
    End Class

    Public Class sala
        Public Property ubic_Codigo As Integer
        Public Property sala_Codigo As Integer
        Public Property sala_Descripcion As String
        Public Property sala_CodigoInterno As String
    End Class

    Public Class area
        Public Property ubic_Codigo As Integer
        Public Property sala_Codigo As Integer
        Public Property area_Codigo As Integer
        Public Property area_Descripcion As String
        Public Property area_CodigoInterno As String
        Public Property unfu_Codigo As Integer
        Public Property depo_Codigo As Integer
    End Class

    Public Class cama
        Public Property ubic_Codigo As Integer
        Public Property sala_Codigo As Integer
        Public Property area_Codigo As Integer
        Public Property cama_Codigo As Integer
        Public Property cama_Descripcion As String
        Public Property cama_CodigoInterno As String
        Public Property teca_Codigo As Integer
        Public Property sepe_Codigo As Integer
        Public Property cama_NroInterno As String
    End Class


    Public Class respuesta
        Public Property estado As String
        Public Property mensaje As String
        Public Property ok As Boolean
    End Class

End Class
