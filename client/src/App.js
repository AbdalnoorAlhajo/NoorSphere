import "./App.css";
import Landing from "./components/Landing";
import { Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Login from "./components/Login";
// import { Provider } from "react-redux";
// import store from "./redux/store";
import Home from "./components/Home";
import SideNavbar from "./components/SideNavbar";
import Posts from "./components/Posts";
import Developers from "./components/Developers";
import Register from "./components/Register";
import Settings from "./components/Settings";
import EditAccount from "./components/AddAndEdit/EditAccount";
import AddEducation from "./components/AddAndEdit/AddEducation";
import AddExperience from "./components/AddAndEdit/AddExperience";
import ShowDeveloper from "./components/ShowDevoloper";
import Discussion from "./components/Discussion";
import { TokenProvider } from "./components/TokenContext";

function App() {
  return (
    <TokenProvider>
      <Routes>
        {/* Authntication */}
        <Route path="/" element={<Navbar />}>
          <Route index element={<Landing />} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
        </Route>

        {/* Home */}
        <Route path="/home" element={<SideNavbar />}>
          <Route index element={<Home />} />
        </Route>

        {/* Posts */}
        <Route path="/posts" element={<SideNavbar />}>
          <Route index element={<Posts />} />
          <Route path="Discussion/:id" element={<Discussion />} />
        </Route>

        {/* Settings */}
        <Route path="/settings" element={<SideNavbar />}>
          <Route index element={<Settings />} />
        </Route>

        {/* Profile */}
        <Route path="/profile" element={<SideNavbar />}>
          <Route path="edit" element={<EditAccount />} />
          <Route path="addEducation" element={<AddEducation />} />
          <Route path="addExperience" element={<AddExperience />} />
        </Route>

        {/* Developer */}
        <Route path="/developers" element={<SideNavbar />}>
          <Route index element={<Developers />} />
          <Route path=":id" element={<ShowDeveloper />} />
        </Route>
      </Routes>
    </TokenProvider>
  );
}

export default App;
