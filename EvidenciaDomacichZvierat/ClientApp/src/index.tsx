import { ThemeProvider } from '@material-ui/styles'
import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter } from 'react-router-dom'
import { ThemeProvider as StyledThemeProvider } from 'styled-components'
import App from './App'
import './index.css'
import theme from './theme'

ReactDOM.render(
  <React.StrictMode>
    <BrowserRouter>
      <StyledThemeProvider theme={theme.palette}>
        <ThemeProvider theme={theme}>
          <App />
        </ThemeProvider>
      </StyledThemeProvider>
    </BrowserRouter>
  </React.StrictMode>,
  document.getElementById('root')
)
