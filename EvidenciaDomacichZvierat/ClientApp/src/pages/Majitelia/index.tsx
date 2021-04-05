import { useState } from 'react'
import { Link } from 'react-router-dom'
import { KeyboardArrowRight } from '@material-ui/icons'
import {
  Avatar,
  Box,
  Button,
  Checkbox,
  CircularProgress,
  Container,
  createStyles,
  List,
  makeStyles,
  MenuItem,
  Theme,
  Typography
} from '@material-ui/core'

import useFetch from '../../hooks/useFetch'
import MajitelStatisticsDialog from './MajitelStatisticsDialog'

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      marginTop: '30px',
      width: '100%',
      backgroundColor: theme.palette.background.paper
    }
  })
)

interface IMajitel {
  id: number
  meno: string
  priezvisko: string
}

const Majitelia = () => {
  const { data, loading, error } = useFetch<IMajitel[]>({ url: '/api/majitelia' })
  const [selectedIds, setSelectedIds] = useState<number[]>([])
  const [statisticsOpen, setStatisticsOpen] = useState(false)
  const styles = useStyles()

  if (loading) return <CircularProgress color='secondary' />
  if (error) return <div>err</div>

  const majitelia = data!

  const handleSelected = (checked: boolean, id: number) => {
    if (checked) return setSelectedIds(prev => [...prev, id])

    setSelectedIds(prev => prev.filter(x => x !== id))
  }

  return (
    <Container maxWidth='md'>
      <Box marginY='30px'>
        <Typography variant='h5'>Majitelia</Typography>
      </Box>

      <List className={styles.root}>
        {majitelia.map(x => (
          <Box display='flex' key={x.id}>
            <Box padding='5px 0 5px 5px'>
              <Checkbox
                value={selectedIds.includes(x.id)}
                onChange={(_, value) => handleSelected(value, x.id)}
              />
            </Box>

            <Box width='100%'>
              <Link to={`/majitelia/${x.id}`}>
                <MenuItem>
                  <Avatar />

                  <Box padding='10px'>{x.meno + ' ' + x.priezvisko}</Box>

                  <Box marginLeft='auto'>
                    <KeyboardArrowRight />
                  </Box>
                </MenuItem>
              </Link>
            </Box>
          </Box>
        ))}
      </List>

      <Box display='flex' flexDirection='row-reverse' marginTop='30px'>
        <Button
          variant='contained'
          color='secondary'
          disabled={!selectedIds.length}
          onClick={() => setStatisticsOpen(true)}
        >
          Zobrazit informacie
        </Button>
      </Box>

      {statisticsOpen && (
        <MajitelStatisticsDialog
          majitelIds={selectedIds}
          onClose={() => setStatisticsOpen(false)}
        />
      )}
    </Container>
  )
}

export default Majitelia
