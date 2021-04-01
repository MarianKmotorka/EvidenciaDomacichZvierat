import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Box,
  Button,
  Card,
  CardContent,
  Dialog,
  MenuItem
} from '@material-ui/core'
import { Pets } from '@material-ui/icons'
import moment from 'moment'
import { useState } from 'react'
import { IMacka } from '../../types'
import NameValueRow from '../NameValueRow'

interface IProps {
  data: IMacka
}

const Macka = ({ data }: IProps) => {
  const [open, setOpen] = useState(false)

  return (
    <>
      <Card elevation={0}>
        <MenuItem onClick={() => setOpen(true)}>
          <CardContent>
            <Box display='flex' alignItems='center' gridGap='10px'>
              <Pets color='secondary' />
              <p>Macka</p>
            </Box>

            <p>{data.meno}</p>
          </CardContent>
        </MenuItem>
      </Card>

      <Dialog open={open} onClose={() => setOpen(false)}>
        <Box minWidth={400}>
          <CardContent>
            <NameValueRow name='Meno' value={data.meno} />
            <NameValueRow
              name='Datum narodenia'
              value={moment(data.datumNarodenia).format('DD.MM.yyyy')}
            />
            <NameValueRow name='Pocet krmeni' value={data.pocetKrmeni} />
            <NameValueRow name='Chyta mysi' value={data.chytaMysi ? 'Ano' : 'Nie'} />

            <Button variant='outlined' color='secondary'>
              Nakrmit
            </Button>
          </CardContent>
        </Box>
      </Dialog>
    </>
  )
}

export default Macka
