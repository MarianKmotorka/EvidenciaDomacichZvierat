import { useCallback, useEffect, useState } from 'react'

interface IUseFetchParameters {
  url: string
  method?: 'GET' | 'POST' | 'DELETE' | 'PUT' | 'PATCH'
  body?: any
}

const useFetch = <T>({ url, method = 'GET', body }: IUseFetchParameters) => {
  const [data, setData] = useState<T>()
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<any>()

  const bodyString = JSON.stringify(body)

  const fetchData = useCallback(async () => {
    const res = await fetch(url, {
      method,
      body: bodyString,
      headers: {
        'Content-Type': 'application/json'
      }
    })
    const json = await res.json()

    if (res.ok) setData(json)
    else setError(json)
    setLoading(false)
  }, [url, method, bodyString])

  useEffect(() => {
    fetchData()
  }, [fetchData])

  return { data, loading, error, setData }
}

export default useFetch
