import { Route, Routes, useLocation } from "react-router-dom";
import Footer from "./layout/Footer";
import NavBar from "./layout/NavBar";
import CapacityForm from "./pages/Form/CapacityForm";
import DistancesForm from "./pages/Form/DistancesForm";
import FinalPage from "./pages/Form/FinalPage";
import LocationsForm from "./pages/Form/LocationsForm";
import ObjectForm from "./pages/Form/ObjectsForm";
import Maps from "./pages/Map/Nodes";
import StartEndTrucks from "./pages/Form/StartEndTrucks";
import StartEndObjects from "./pages/Form/StartEndObjects";
import PickUpDropOff from "./pages/Form/PickUpDropOff";
import Home from "./pages/Home/Home";
import InitialForm from "./pages/Form/InitialForm";
import BenchMark from "./pages/BenchMark/BenchMark";
import Settings from "./pages/Settings/Settings";
import AdvancedSettings from "./pages/Settings/AdvancedSettings";

function App() {
  const { pathname } = useLocation();

  return (
    <div className="App p-4 flex flex-col gap-3 justify-between">
      {pathname !== "/map/nodes" && <NavBar />}
      <div className={`flex justify-center items-center `}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="benchmark" element={<BenchMark />} />
          <Route path="initial" element={<InitialForm />} />
          <Route path="locations" element={<LocationsForm />} />
          <Route path="distances" element={<DistancesForm />} />
          <Route path="capacities" element={<CapacityForm />} />
          <Route path="truck-details" element={<StartEndTrucks />} />
          <Route path="objects" element={<ObjectForm />} />
          <Route path="objects-details" element={<StartEndObjects />} />
          <Route path="objects-pick-drop" element={<PickUpDropOff />} />
          <Route path="final" element={<FinalPage />} />
          <Route path="settings">
            <Route index element={<Settings />} />
            <Route path="/settings/advanced" element={<AdvancedSettings />} />
          </Route>
          <Route path="map">
            <Route path="/map/nodes" element={<Maps />} />
          </Route>
        </Routes>
      </div>
      {pathname !== "/map/nodes" && <Footer />}
    </div>
  );
}

export default App;
