import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Box,
  Button,
  Card,
  CardActions,
  CardContent,
  Dialog,
  MenuItem
} from '@material-ui/core'
import { Pets } from '@material-ui/icons'
import moment from 'moment'
import { useState } from 'react'
import { IPes } from '../../types'
import NameValueRow from '../NameValueRow'

interface IProps {
  data: IPes
}

const Pes = ({ data }: IProps) => {
  const [open, setOpen] = useState(false)

  return (
    <>
      <Card elevation={0}>
        <MenuItem onClick={() => setOpen(true)}>
          <CardContent>
            <Box display='flex' alignItems='center' gridGap='10px'>
              <Pets color='primary' />
              <p>Pes</p>
            </Box>

            <p>{data.meno}</p>
          </CardContent>
        </MenuItem>
      </Card>

      <Dialog open={open} onClose={() => setOpen(false)}>
        <Box minWidth={400}>
          <CardContent>
            <NameValueRow
              name='Datum narodenia'
              value={moment(data.datumNarodenia).format('DD.MM.yyyy')}
            />
            <NameValueRow name='Pocet krmeni' value={data.pocetKrmeni} />
            <NameValueRow name='Predpokladany vzrast' value={data.predpokladanyVzrastCm} />
            <NameValueRow name='Uroven vycviku' value={data.urovenVycviku} />

            <Button variant='outlined' color='primary'>
              Nakrmit
            </Button>
          </CardContent>
        </Box>
      </Dialog>
    </>
  )
}

export default Pes
