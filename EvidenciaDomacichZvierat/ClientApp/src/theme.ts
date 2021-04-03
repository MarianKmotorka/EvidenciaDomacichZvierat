import { createMuiTheme } from '@material-ui/core'
import { Palette } from '@material-ui/core/styles/createPalette'

const theme = createMuiTheme({
  palette: {
    primary: {
      main: '#05745d'
    }
  }
})

export default theme

declare module 'styled-components' {
  export interface DefaultTheme extends Palette {}
}
