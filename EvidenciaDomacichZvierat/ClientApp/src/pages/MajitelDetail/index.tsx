import { useCallback } from 'react'
import { useParams } from 'react-router'
import { Link } from 'react-router-dom'
import { KeyboardArrowLeft } from '@material-ui/icons'
import { Avatar, Box, Button, CircularProgress, Container, Grid } from '@material-ui/core'

import Pes from '../../components/Pes'
import Macka from '../../components/Macka'
import useFetch from '../../hooks/useFetch'
import NameValueRow from '../../components/NameValueRow'
import { IMajitelDetail, ZvieraEnum } from '../../types'

const MajitelDetail = () => {
  const { id } = useParams<{ id: string }>()
  const { data, loading, error, setData } = useFetch<IMajitelDetail>({ url: `/api/majitel/${id}` })

  const handleNakrmit = useCallback(
    async (zvieraId: number) => {
      const response = await fetch(`/api/zviera/${zvieraId}/nakrmit`, { method: 'POST' })
      if (!response.ok) return // Show error toast

      setData(prev => {
        if (!prev) return prev

        const updated = prev.zvierata.map(x => {
          if (x.id === zvieraId) return { ...x, pocetKrmeni: x.pocetKrmeni + 1 }
          return x
        })

        return { ...prev, zvierata: [...updated] }
      })
    },
    [setData]
  )

  if (loading) return <CircularProgress color='secondary' />
  if (error) return <h2>Error</h2>

  const majitel = data!

  return (
    <Container maxWidth='md'>
      <Box marginY='15px'>
        <Link to='/'>
          <Button size='large' startIcon={<KeyboardArrowLeft />}>
            Späť
          </Button>
        </Link>
      </Box>

      <Box marginBottom='30px' display='flex' alignItems='center'>
        <Box marginRight='10px'>
          <Avatar />
        </Box>

        <h2>{majitel.meno + ' ' + majitel.priezvisko}</h2>
      </Box>

      <NameValueRow name='Vek' value={majitel.vek} />
      <NameValueRow name='Priemerny vek zvierat' value={majitel.priemernyVekZvierat} />

      <Box marginY='30px'>
        <Grid container spacing={1}>
          {majitel.zvierata.map(x => (
            <Grid item key={x.id} xs={12} md={6}>
              {x.type === ZvieraEnum.Pes ? (
                <Pes data={x} onNakrmit={handleNakrmit} />
              ) : (
                <Macka data={x} onNakrmit={handleNakrmit} />
              )}
            </Grid>
          ))}
        </Grid>
      </Box>
    </Container>
  )
}

export default MajitelDetail
