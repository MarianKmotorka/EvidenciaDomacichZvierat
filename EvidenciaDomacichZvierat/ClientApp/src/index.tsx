import ReactDOM from 'react-dom'
import { BrowserRouter } from 'react-router-dom'
import { ThemeProvider } from '@material-ui/styles'
import { ThemeProvider as StyledThemeProvider } from 'styled-components'

import './index.css'
import App from './App'
import theme from './theme'

ReactDOM.render(
  <BrowserRouter>
    <StyledThemeProvider theme={theme.palette}>
      <ThemeProvider theme={theme}>
        <App />
      </ThemeProvider>
    </StyledThemeProvider>
  </BrowserRouter>,
  document.getElementById('root')
)
