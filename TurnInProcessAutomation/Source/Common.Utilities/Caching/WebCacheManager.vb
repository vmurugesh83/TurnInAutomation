Imports System.Web

Namespace Caching

    Public Class WebCacheManager

        ''' <summary>
        ''' Tests for the requested value in the session cache.
        ''' </summary>
        ''' <typeparam name="t"></typeparam>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckCache(Of t)(ByVal key As String) As t
            Dim returnValue As t

            If Not HttpContext.Current Is Nothing Then
                Dim oValue As Object = HttpContext.Current.Cache.Get(key)

                If TypeOf oValue Is t Then
                    returnValue = DirectCast(oValue, t)
                End If
            End If

            Return returnValue
        End Function

        ''' <summary>
        ''' Cache items to the session cache for a set number of minutes.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <param name="item"></param>
        ''' <param name="cacheMinutes"></param>
        ''' <remarks></remarks>
        Public Shared Sub CacheItem(ByVal key As String, ByVal item As Object, ByVal cacheMinutes As Integer)
            If Not HttpContext.Current Is Nothing AndAlso Not item Is Nothing Then
                Dim cache As System.Web.Caching.Cache = HttpContext.Current.Cache
                Dim existingItem As Object = cache.Get(key)

                'if the item is already cached
                If Not existingItem Is Nothing Then
                    cache.Remove(key)
                End If

                cache.Add(key, item, Nothing, DateTime.Now.AddMinutes(cacheMinutes), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, Nothing)

            End If
        End Sub

        ''' <summary>
        ''' Removes an item from the cache.
        ''' </summary>
        ''' <param name="key"></param>
        ''' <remarks></remarks>
        Public Shared Sub ExpireCacheItem(ByVal key As String)
            If Not HttpContext.Current Is Nothing Then
                HttpContext.Current.Cache.Remove(key)
            End If
        End Sub
    End Class

End Namespace