Public Class cpt
    Public Property cept_Codigo As Long
    Public Property cept_CodigoInterno As String
    Public Property cept_Descripcion As String
    Public Property cgru_Codigo As Long
    Public Property cgru_Descripcion As String
    Public Property csub_Codigo As Long
    Public Property csub_Descripcion As String
    Public Property test_CodigoCPT As Integer
    Public Property cept_Agendable As String
    Public Property cept_Tiempo As Integer
    Public Property cept_Consulta As String
    Public Property cept_HospitalDia As String
End Class

Public Class respuestaCPTs
    Public Property estado As String
    Public Property mensaje As String
    Public Property ok As Boolean
    Public Property cpts As List(Of cpt)
End Class

Public Class nomencladorNacional
    Public Property tnom_Codigo As Integer
    Public Property tnom_Descripcion As String
    Public Property nome_Codigo As String
    Public Property nome_Descripcion As String
    Public Property tpre_Codigo As Integer
    Public Property tpre_Descricpion As String
    Public Property tpre_Abreviatura As String
    Public Property nome_SubDescripcion As String
    Public Property nome_CodigoHomologado As String
    Public Property nome_Nomenclador As String
    Public Property nome_Estado As String
    Public Property mone_Codigo As Integer
    Public Property mone_Descripcion As String
    Public Property mone_Denominacion As String
    Public Property mone_Simbolo As String
    Public Property nome_Procesamiento As String
    Public Property nome_SinCargo As String
End Class

Public Class respuestaNomencladorNacional
    Public Property estado As String
    Public Property mensaje As String
    Public Property ok As Boolean
    Public Property nomencladorNacional As List(Of nomencladorNacional)
End Class

Public Class cptNomenclador
    Public Property tnom_Codigo As Integer
    Public Property nome_Codigo As String
    Public Property cpet_Codigo As Long
    Public Property cobe_Codigo As Long
    Public Property cptn_Cantidad As Integer
End Class

Public Class respuestaCptNomenclador
    Public Property estado As String
    Public Property mensaje As String
    Public Property ok As Boolean
    Public Property cptNomencladores As List(Of cptNomenclador)
End Class

