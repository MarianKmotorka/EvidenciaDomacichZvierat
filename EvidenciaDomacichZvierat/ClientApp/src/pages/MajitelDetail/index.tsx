import { Avatar, Box, Button, CircularProgress, Container, Grid } from '@material-ui/core'
import { KeyboardArrowLeft } from '@material-ui/icons'
import { useParams } from 'react-router'
import { Link } from 'react-router-dom'
import Macka from '../../components/Macka'
import NameValueRow from '../../components/NameValueRow'
import Pes from '../../components/Pes'
import useFetch from '../../hooks/useFetch'
import { IMajitelDetail, ZvieraEnum } from '../../types'

const MajitelDetail = () => {
  const { id } = useParams<{ id: string }>()
  const { data, loading, error, setData } = useFetch<IMajitelDetail>({ url: `/api/majitel/${id}` })

  if (loading) return <CircularProgress color='secondary' />
  if (error) return <h2>NOT FOUND</h2>

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
              {x.type === ZvieraEnum.Pes ? <Pes data={x} /> : <Macka data={x} />}
            </Grid>
          ))}
        </Grid>
      </Box>
    </Container>
  )
}

export default MajitelDetail
